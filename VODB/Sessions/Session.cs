using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VODB.DbLayer;
using VODB.DbLayer.DbResults;

namespace VODB.Sessions
{
    class Session : ISession, IInternalSession
    {
        private DbConnection _connection;
        private IInternalTransaction _Transaction;
        private IDbConnectionCreator _Creator;
        
        public Session(IDbConnectionCreator creator, IInternalTransaction transaction)
        {
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
            throw new NotImplementedException();
        }

        public Task<IDbQueryResult<TEntity>> AsyncGetAll<TEntity>() where TEntity : class, new()
        {
            throw new NotImplementedException();
        }

        public TEntity GetById<TEntity>(TEntity entity) where TEntity : class, new()
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> AsyncGetById<TEntity>(TEntity entity) where TEntity : class, new()
        {
            throw new NotImplementedException();
        }

        public TEntity Insert<TEntity>(TEntity entity) where TEntity : class, new()
        {
            throw new NotImplementedException();
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class, new()
        {
            throw new NotImplementedException();
        }

        public TEntity Update<TEntity>(TEntity entity) where TEntity : class, new()
        {
            throw new NotImplementedException();
        }

        public int Count<TEntity>() where TEntity : class, new()
        {
            throw new NotImplementedException();
        }

        public bool Exists<TEntity>(TEntity entity) where TEntity : class, new()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
