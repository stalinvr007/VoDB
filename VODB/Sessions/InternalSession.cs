using System;
using System.Data;
using System.Data.Common;
using VODB.Core;
using VODB.Core.Execution.Executers;
using VODB.Core.Execution.Executers.DbResults;
using VODB.Core.Infrastructure;
using VODB.Core.Loaders;
using VODB.Core.Loaders.Factories;
using VODB.DbLayer;

namespace VODB.Sessions
{
    internal class InternalSession : IInternalSession
    {
        private readonly IStatementExecuter<int> _CountByIdExecuter;
        private readonly IStatementExecuter<int> _CountExecuter;
        private readonly IStatementExecuter<int> _DeleteExecuter;
        private readonly IEntityFactory _EntityFactory;
        private readonly IEntityLoader _EntityLoader;
        private readonly IStatementExecuter<object> _IdentityExecuter;
        private readonly IStatementExecuter<int> _InsertExecuter;
        private readonly IQueryResultGetter _QueryResultGetter;
        private readonly IStatementExecuter<DbDataReader> _SelectByIdExecuter;
        private readonly IStatementExecuter _StatementExecuter;
        private readonly IStatementExecuter<int> _UpdateExecuter;
        private IDbConnectionCreator _Creator;
        private IInternalTransaction _Transaction;
        private DbConnection _connection;

        public InternalSession(
            IDbConnectionCreator creator,
            IInternalTransaction transaction,
            [Bind(Commands.Insert)] IStatementExecuter<int> insertExecuter,
            [Bind(Commands.Update)] IStatementExecuter<int> updateExecuter,
            [Bind(Commands.Delete)] IStatementExecuter<int> deleteExecuter,
            [Bind(Commands.Count)] IStatementExecuter<int> countExecuter,
            [Bind(Commands.CountById)] IStatementExecuter<int> countByIdExecuter,
            [Bind(Commands.SelectById)] IStatementExecuter<DbDataReader> selectByIdExecuter,
            [Bind(Commands.Identity)] IStatementExecuter<Object> IdentityExecuter,
            IStatementExecuter statementExecuter,
            IQueryResultGetter queryResultGetter,
            IEntityLoader entityLoader,
            IEntityFactory entityFactory)
        {
            _EntityFactory = entityFactory;
            _EntityLoader = entityLoader;
            _QueryResultGetter = queryResultGetter;
            _SelectByIdExecuter = selectByIdExecuter;
            _IdentityExecuter = IdentityExecuter;
            _StatementExecuter = statementExecuter;
            _CountByIdExecuter = countByIdExecuter;
            _CountExecuter = countExecuter;
            _DeleteExecuter = deleteExecuter;
            _UpdateExecuter = updateExecuter;
            _InsertExecuter = insertExecuter;
            _Creator = creator;
            _Transaction = transaction;
        }

        private bool InTransaction
        {
            get { return !_Transaction.Ended; }
        }

        #region IInternalSession Members

        public DbCommand RefreshCommand(DbCommand command)
        {
            Open();
            command.Connection = _connection;
            return command;
        }

        public DbCommand CreateCommand()
        {
            CreateConnection();

            return InTransaction
                       ? _Transaction.CreateCommand()
                       : _connection.CreateCommand();
        }

        public void Open()
        {
            CreateConnection();

            if (_connection.State == ConnectionState.Open)
            {
                return;
            }
            _connection.Open();
        }

        public void Close()
        {
            if (_connection == null || _connection.State == ConnectionState.Closed || InTransaction)
            {
                return;
            }
            _connection.Close();
            _connection = null;
        }

        public void Dispose()
        {
            Close();
            if (_Transaction != null)
            {
                _Transaction.Dispose();
                _Transaction = null;
            }

            if (_connection == null)
            {
                return;
            }

            _connection.Dispose();
            _connection = null;
            _Creator = null;
        }

        #endregion

        #region ISession Implementation

        public ITransaction BeginTransaction()
        {
            Open();
            return _Transaction.BeginTransaction(this, _connection);
        }

        public void ExecuteTSql(string SqlStatements)
        {
            _StatementExecuter.Execute(SqlStatements, this);
            Close();
        }

        public IDbQueryResult<TEntity> GetAll<TEntity>() where TEntity : class, new()
        {
            return _QueryResultGetter.GetQueryResult<TEntity>(this, _EntityLoader, _EntityFactory);
        }

        public TEntity GetById<TEntity>(TEntity entity) where TEntity : class, new()
        {
            if (entity == null)
            {
                return null;
            }

            DbDataReader reader = _SelectByIdExecuter.Execute(entity, this);
            try
            {
                if (reader.Read())
                {
                    var newEntity = _EntityFactory.Make(entity.GetType(), this) as TEntity;
                    _EntityLoader.Load(newEntity, this, reader);
                    return newEntity;
                }
            }
            finally
            {
                reader.Close();
                Close();
            }
            return null;
        }

        public TEntity Insert<TEntity>(TEntity entity) where TEntity : class, new()
        {
            _InsertExecuter.Execute(entity, this);

            Field field = entity.GetTable().IdentityField;
            if (field != null)
            {
                field.SetValue(entity, Convert.ChangeType(_IdentityExecuter.Execute(entity, this), field.FieldType));
            }

            Close();
            return entity;
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class, new()
        {
            _DeleteExecuter.Execute(entity, this);
            Close();
        }

        public TEntity Update<TEntity>(TEntity entity) where TEntity : class, new()
        {
            _UpdateExecuter.Execute(entity, this);
            Close();
            return entity;
        }

        public int Count<TEntity>() where TEntity : class, new()
        {
            try
            {
                return _CountExecuter.Execute(new TEntity(), this);
            }
            finally
            {
                Close();
            }
        }

        public bool Exists<TEntity>(TEntity entity) where TEntity : class, new()
        {
            try
            {
                return _CountByIdExecuter.Execute(entity, this) > 0;
            }
            finally
            {
                Close();
            }
        }

        #endregion

        private void CreateConnection()
        {
            if (_connection == null)
            {
                _connection = _Creator.Create();
            }
        }
    }
}