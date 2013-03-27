using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VODB.Core.Execution.Executers.DbResults;
using VODB.DbLayer;
using VODB.EntityMapping;
using VODB.EntityTranslation;
using VODB.Infrastructure;

namespace VODB.Sessions
{

    class V2Session : ISession
    {

        private IVodbConnection _Connection;
        private readonly IEntityTranslator _Translator;
        private readonly IEntityMapper _Mapper;

        public V2Session(IVodbConnection connection, IEntityTranslator translator, IEntityMapper mapper)
        {
            _Mapper = mapper;
            _Translator = translator;
            _Connection = connection;
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

        #region ISession Implementation

        public ITransaction BeginTransaction()
        {
            return _Connection.BeginTransaction();
        }

        public void ExecuteTSql(string SqlStatements)
        {
            throw new NotImplementedException();
        }

        public IDbQueryResult<TEntity> GetAll<TEntity>() where TEntity : class, new()
        {
            throw new NotImplementedException();
        }

        public TEntity GetById<TEntity>(TEntity entity) where TEntity : class, new()
        {

            System.Data.IDataReader reader = null;
            try
            {
                var table = GetTable<TEntity>();
                var command = table.GetSelectByIdCommand(_Connection);
                SetKeyValues<TEntity>(entity, table, command);
                reader = _Connection.ExecuteReader(command);

                return reader.Read() ? _Mapper.Map(entity, table, reader) : null;
            }
            finally
            {
                reader.Close();
            }

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
    }
}
