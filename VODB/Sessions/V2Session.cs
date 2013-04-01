using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VODB.Core.Execution.Executers.DbResults;
using VODB.DbLayer;
using VODB.EntityMapping;
using VODB.EntityTranslation;
using VODB.Infrastructure;
using VODB.QueryCompiler;
using ConcurrentReader;
using VODB.Core.Loaders.Factories;

namespace VODB.Sessions
{

    class V2Session : IInternalSession
    {

        private IVodbConnection _Connection;
        private readonly IEntityTranslator _Translator;
        private readonly IEntityMapper _Mapper;
        private readonly IEntityFactory _EntityFactory;

        public V2Session(IVodbConnection connection, IEntityTranslator translator, IEntityMapper mapper, IEntityFactory entityFactory)
        {
            _Mapper = mapper;
            _Translator = translator;
            _Connection = connection;
            _EntityFactory = entityFactory;
        }

        private static void SetKeyValues<TEntity>(TEntity entity, ITable table, IVodbCommand command)
        {
            command.SetParametersValues(
                table.Keys.Select(f => f.GetFieldFinalValue(entity)).ToArray()
            );
        }

        private static void SetFieldValues<TEntity>(TEntity entity, ITable table, IVodbCommand command)
        {
            command.SetParametersValues(
                table.Fields.Where(f => !f.IsIdentity).Select(f => f.GetFieldFinalValue(entity)).ToArray()
            );
        }

        private static void SetAllFieldValues<TEntity>(TEntity entity, ITable table, IVodbCommand command)
        {
            command.SetParametersValues(
                table.Fields.Where(f => !f.IsIdentity).Select(f => f.GetFieldFinalValue(entity))
                .Concat(table.Keys.Select(f => f.GetFieldFinalValue(entity)))
                .ToArray()
            );
        }

        private ITable GetTable<TEntity>() where TEntity : class, new()
        {
            return _Translator.Translate(typeof(TEntity));
        }

        private IEnumerable<TEntity> ExecuteQuery<TEntity>(IDataReader reader, ITable table)
        {
            try
            {
                while (reader.Read())
                {
                    yield return _Mapper.Map(_EntityFactory.Make<TEntity>(this), table, reader);
                }
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
            return new QueryCompiler<TEntity>(_Translator, this);
        }

        public IEnumerable<TEntity> ExecuteQuery<TEntity>(IQuery<TEntity> query, params Object[] args) where TEntity : class, new()
        {
            var level = 0;
            var table = GetTable<TEntity>();
            var command = _Connection.MakeCommand(query.Compile(ref level))
                .SetParametersValues(args);

            return ExecuteQuery<TEntity>(_Connection.ExecuteReader(command), table);
        }

        public TEntity GetById<TEntity>(TEntity entity) where TEntity : class, new()
        {
            var table = GetTable<TEntity>();
            var command = table.GetSelectByIdCommand(_Connection);
            SetKeyValues<TEntity>(entity, table, command);

            return ExecuteQuery<TEntity>(_Connection.ExecuteReader(command), table).FirstOrDefault();
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
            SetKeyValues<TEntity>(entity, table, command);

            return _Connection.ExecuteNonQuery(command) == 1;
        }

        public TEntity Update<TEntity>(TEntity entity) where TEntity : class, new()
        {
            var table = GetTable<TEntity>();
            var command = table.GetUpdateCommand(_Connection);
            SetAllFieldValues<TEntity>(entity, table, command);

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
            SetKeyValues<TEntity>(entity, table, command);

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
    }
}
