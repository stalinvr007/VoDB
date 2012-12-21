using Ninject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VODB.Core;
using VODB.Core.Execution.Executers;
using VODB.DbLayer;
using VODB.Core.Execution.Executers.DbResults;
using VODB.Core.Loaders;
using VODB.Core.Loaders.Factories;

namespace VODB.Sessions
{
    class InternalSession : ISession, IInternalSession
    {
        private DbConnection _connection;
        private IInternalTransaction _Transaction;
        private IDbConnectionCreator _Creator;
        private readonly IStatementExecuter<int> _InsertExecuter;
        private readonly IStatementExecuter<int> _UpdateExecuter;
        private readonly IStatementExecuter<int> _DeleteExecuter;
        private readonly IStatementExecuter<int> _CountExecuter;
        private readonly IStatementExecuter<int> _CountByIdExecuter;
        private readonly IStatementExecuter<DbDataReader> _SelectByIdExecuter;
        private readonly IQueryResultGetter _QueryResultGetter;
        private readonly IEntityLoader _EntityLoader;
        private readonly IEntityFactory _EntityFactory;
        
        public InternalSession(
            IDbConnectionCreator creator,
            IInternalTransaction transaction,
            [Bind(Commands.Insert)] IStatementExecuter<int> insertExecuter,
            [Bind(Commands.Update)] IStatementExecuter<int> updateExecuter,
            [Bind(Commands.Delete)] IStatementExecuter<int> deleteExecuter,
            [Bind(Commands.Count)] IStatementExecuter<int> countExecuter,
            [Bind(Commands.CountById)] IStatementExecuter<int> countByIdExecuter,
            [Bind(Commands.SelectById)] IStatementExecuter<DbDataReader> selectByIdExecuter,
            IQueryResultGetter queryResultGetter,
            IEntityLoader entityLoader,
            IEntityFactory entityFactory)
        {
            _EntityFactory = entityFactory;
            _EntityLoader = entityLoader;
            _QueryResultGetter = queryResultGetter;
            _SelectByIdExecuter = selectByIdExecuter;
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

        #endregion

        private void CreateConnection()
        {
            if (_connection == null)
            {
                _connection = _Creator.Create();
            }
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

        #region ISession Implementation

        public ITransaction BeginTransaction()
        {
            return _Transaction.BeginTransaction(_connection);
        }

        public void ExecuteTSql(string SqlStatements)
        {
            throw new NotImplementedException();
        }

        public IDbQueryResult<TEntity> GetAll<TEntity>() where TEntity : class, new()
        {
            return _QueryResultGetter.GetQueryResult<TEntity>(this, _EntityLoader, _EntityFactory);
        }

        public TEntity GetById<TEntity>(TEntity entity) where TEntity : class, new()
        {
            var reader = _SelectByIdExecuter.Execute(entity, this);
            if (reader.Read())
            {
                TEntity newEntity = _EntityFactory.Make<TEntity>();
                _EntityLoader.Load(newEntity, this, reader);
                return newEntity;
            }
            return null;
        }

        public TEntity Insert<TEntity>(TEntity entity) where TEntity : class, new()
        {
            _InsertExecuter.Execute(entity, this);
            return entity;
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class, new()
        {
            _DeleteExecuter.Execute(entity, this);
        }

        public TEntity Update<TEntity>(TEntity entity) where TEntity : class, new()
        {
            _UpdateExecuter.Execute(entity, this);
            return entity;
        }

        public int Count<TEntity>() where TEntity : class, new()
        {
            return _CountExecuter.Execute<TEntity>(new TEntity(), this);
        }

        public bool Exists<TEntity>(TEntity entity) where TEntity : class, new()
        {
            return _CountByIdExecuter.Execute<TEntity>(entity, this) > 0;
        }

        #endregion
    }
}
