﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using VODB.DbLayer;
using VODB.EntityMapping;
using VODB.EntityTranslation;
using VODB.Infrastructure;
using VODB.QueryCompiler;
using VODB.Core.Loaders.Factories;
using VODB.ExpressionsToSql;
using VODB.Exceptions;
using System.Collections;

namespace VODB.Sessions
{

    class SessionV2 : IInternalSession
    {

        private IVodbConnection _Connection;
        private readonly IEntityTranslator _Translator;
        private readonly IEntityMapper _Mapper;
        private readonly IEntityFactory _EntityFactory;
        
        // Wrapper for the session. 
        private readonly IInternalSession _InternalSession;

        public SessionV2(IVodbConnection connection, IEntityTranslator translator, IEntityMapper mapper, IEntityFactory entityFactory)
        {
            _Mapper = mapper;
            _Translator = translator;
            _Connection = connection;
            _EntityFactory = entityFactory;

            // The session base makes sure every exception will be captured.
            _InternalSession = new SessionBase(this);
        }

        private static QueryParameter CreateParameter<TEntity>(IField f, TEntity entity)
        {
            return new QueryParameter { Value = f.GetFieldFinalValue(entity), type = f.Info.PropertyType, Field = f };
        }
        private static void SetKeyValues<TEntity>(TEntity entity, ITable table, IVodbCommand command)
        {
            command.SetParametersValues(
                table.Keys.Select(f => CreateParameter(f, entity))
            );
        }

        private static void SetFieldValues<TEntity>(TEntity entity, ITable table, IVodbCommand command)
        {
            command.SetParametersValues(
                table.Fields.Where(f => !f.IsIdentity).Select(f => CreateParameter(f, entity))
            );
        }

        private static void SetAllFieldValues<TEntity>(TEntity entity, ITable table, IVodbCommand command)
        {
            command.SetParametersValues(
                table.Fields.Where(f => !f.IsIdentity).Select(f => CreateParameter(f, entity))
                .Concat(table.Keys.Select(f => CreateParameter(f, entity)))
            );
        }

        private ITable GetTable<TEntity>(TEntity entity = null) where TEntity : class, new()
        {
            return _Translator.Translate(GetEntityType(entity));
        }

        private static Type GetEntityType<TEntity>(TEntity entity)
                where TEntity : class, new()
        {
            var entityType = typeof(TEntity);
            if (entityType == typeof(Object))
            {
                entityType = (entity ?? new TEntity()).GetType();
            }
            return entityType;
        }

        private IEnumerable<TEntity> ExecuteQuery<TEntity>(TEntity entity, IDataReader reader, ITable table) where TEntity : class, new()
        {
            var entityType = GetEntityType(entity);

            try
            {
                while (reader.Read())
                {
                    yield return _Mapper.Map(
                        (TEntity)_EntityFactory.Make(entityType, _InternalSession, _Translator),
                        table,
                        reader
                    );
                }
            }
            finally
            {
                reader.Close();
            }
        }

        private IEnumerable<TEntity> ParallelExecuteQuery<TEntity>(TEntity entity, IDataReader reader, ITable table) where TEntity : class, new()
        {
            var entityType = GetEntityType(entity);

            try
            {
                return reader.AsParallel().Transform(t => _Mapper.Map(
                    (TEntity)_EntityFactory.Make(entityType, _InternalSession, _Translator),
                    table,
                    t.Reader)
                );
            }
            finally
            {
                reader.Close();
            }
        }

        #region ISession Implementation

        public ITransaction BeginTransaction()
        {
            return _Connection.BeginTransaction();
        }

        public void ExecuteTSql(string SqlStatements)
        {
            throw new NotImplementedException();
        }

        public IQueryCompilerLevel1<TEntity> GetAll<TEntity>() where TEntity : class, new()
        {
            return QueryStart.From<TEntity>(_InternalSession);
        }

        private static IQuery GetInternalQuery<TEntity>(IQuery<TEntity> query) where TEntity : class, new()
        {
            var internalQuery = query as IQuery;

            if (internalQuery == null)
            {
                throw new ArgumentException("The IQuery<> passed as argument does not implement IQuery", "query");
            }
            return internalQuery;
        }

        public IEnumerable<TEntity> InternalExecuteQuery<TEntity>(IQuery<TEntity> query, params object[] args) where TEntity : class, new()
        {
            var table = GetTable<TEntity>();
            // Make sure the query received is a IQuery implementation.
            var internalQuery = GetInternalQuery<TEntity>(query);

            // Make or get the command.
            var command = internalQuery.CachedCommand ??
                _Connection.MakeCommand(internalQuery.Compile())
                .SetParameters(internalQuery.Parameters);

            // Holds out the command to use later again.
            internalQuery.CachedCommand = command.SetParametersValues(
                args.Select(v => new QueryParameter { Value = v })
            );

            return ParallelExecuteQuery(default(TEntity), _Connection.ExecuteReader(command), table);
        }

        public IEnumerable<TEntity> ExecuteQuery<TEntity>(IQuery<TEntity> query, params Object[] args) where TEntity : class, new()
        {
            // Make sure the query received is a IQuery implementation.
            var internalQuery = GetInternalQuery<TEntity>(query);

            internalQuery.Compile();
            if (internalQuery.Parameters.Count() != args.Length)
            {

                args = args.SelectMany<Object, Object>(val =>
                {
                    Type valueType = val.GetType();
                    if (val != null && valueType != typeof(String) && valueType.GetInterfaces().Contains(typeof(IEnumerable)))
                    {
                        return (val as IEnumerable).Cast<Object>();
                    }
                    return new[] { val };
                }).ToArray();

                if (internalQuery.Parameters.Count() != args.Length)
                {
                    throw new WrongArgumentsException(internalQuery, args);
                }
            }

            // Resets the values.
            var i = 0;
            foreach (var parameter in internalQuery.Parameters)
            {
                parameter.Value = args[i++];
            }


            // Makes a new query in order to enable lazy load.
            return QueryStart.From<TEntity>(_InternalSession, internalQuery.SqlCompiler, internalQuery.Parameters);
        }

        public TEntity GetById<TEntity>(TEntity entity) where TEntity : class, new()
        {
            var table = GetTable(entity);
            var command = table.GetSelectByIdCommand(_Connection);
            SetKeyValues(entity, table, command);

            return ExecuteQuery(entity, _Connection.ExecuteReader(command), table).FirstOrDefault();
        }

        public TEntity Insert<TEntity>(TEntity entity) where TEntity : class, new()
        {
            var table = GetTable<TEntity>();
            var command = table.GetInsertCommand(_Connection);
            SetFieldValues(entity, table, command);

            var id = _Connection.ExecuteScalar(command);

            table.SetIdentityValue(entity, id);

            return entity;
        }

        public bool Delete<TEntity>(TEntity entity) where TEntity : class, new()
        {
            var table = GetTable<TEntity>();
            var command = table.GetDeleteCommand(_Connection);
            SetKeyValues(entity, table, command);

            return _Connection.ExecuteNonQuery(command) == 1;
        }

        public TEntity Update<TEntity>(TEntity entity) where TEntity : class, new()
        {
            var table = GetTable<TEntity>();
            var command = table.GetUpdateCommand(_Connection);
            SetAllFieldValues(entity, table, command);

            return _Connection.ExecuteNonQuery(command) == 1 ? entity : null;
        }

        public int Count<TEntity>() where TEntity : class, new()
        {
            var command = GetTable<TEntity>().GetCountCommand(_Connection);
            return (int)_Connection.ExecuteScalar(command);
        }

        public bool Exists<TEntity>(TEntity entity) where TEntity : class, new()
        {
            var table = GetTable<TEntity>();
            var command = table.GetCountByIdCommand(_Connection);
            SetKeyValues(entity, table, command);

            return (int)_Connection.ExecuteScalar(command) == 1;
        }

        public void Dispose()
        {
            if (_Connection == null)
            {
                return;
            }

            _Connection.Dispose();
            _Connection = null;
        }

        #endregion

        public System.Data.Common.DbCommand CreateCommand()
        {
            throw new NotImplementedException();
        }

        public System.Data.Common.DbCommand RefreshCommand(System.Data.Common.DbCommand command)
        {
            throw new NotImplementedException();
        }

        public void Open()
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }



        public override string ToString()
        {
            return "Session V2.0";
        }


        public int ExecuteNonQuery(string command, IEnumerable<IQueryParameter> args)
        {
            return _Connection.ExecuteNonQuery(
                _Connection.MakeCommand(command).SetParameters(args)
            );
        }

        public IDataReader ExecuteReader(string command, IEnumerable<IQueryParameter> args)
        {
            return _Connection.ExecuteReader(
                _Connection.MakeCommand(command).SetParameters(args)
            );
        }

        public object ExecuteScalar(string command, IEnumerable<IQueryParameter> args)
        {
            return _Connection.ExecuteScalar(
                _Connection.MakeCommand(command).SetParameters(args)
            );
        }


        public string DataBaseName
        {
            get { return _Connection.DataBaseName; }
        }
    }
}
