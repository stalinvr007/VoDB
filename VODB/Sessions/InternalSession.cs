using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using VODB.DbLayer;
using VODB.Extensions;

namespace VODB.Sessions
{
    /// <summary>
    /// Represents a connection Session.
    /// </summary>
    internal abstract class InternalSession : IInternalSession, ISession, IDisposable
    {
        private IDbConnectionCreator _creator;
        private DbConnection _connection;
        private Transaction transaction;

        /// <summary>
        /// Initializes a new instance of the <see cref="InternalSession" /> class.
        /// </summary>
        /// <param name="creator">The creator.</param>
        internal InternalSession(IDbConnectionCreator creator = null)
        {
            _creator = creator ?? new SqlConnectionCreator();
        }

        private bool InTransaction
        {
            get { return transaction != null; }
        }

        #region ISession Members

        public ITransaction BeginTransaction()
        {
            CreateConnection();

            if (transaction != null)
            {
                transaction.BeginNestedTransaction();
            }
            else
            {
                transaction = new Transaction(_connection.BeginTransaction());
            }

            return transaction;
        }

        public abstract IEnumerable<TEntity> GetAll<TEntity>() where TEntity : DbEntity, new();

        public Task<IEnumerable<TEntity>> AsyncGetAll<TEntity>() where TEntity : DbEntity, new()
        {
            return new Task<IEnumerable<TEntity>>(GetAll<TEntity>).RunAsync();
        }

        public abstract TEntity GetById<TEntity>(TEntity entity) where TEntity : DbEntity, new();

        public Task<TEntity> AsyncGetById<TEntity>(TEntity entity) where TEntity : DbEntity, new()
        {
            return new Task<TEntity>(() => GetById(entity)).RunAsync();
        }

        #endregion

        #region IInternalSession Members

        public DbCommand CreateCommand()
        {
            CreateConnection();

            return InTransaction
                       ? transaction.CreateCommand()
                       : _connection.CreateCommand();
        }


        public void Open()
        {
            CreateCommand();

            if (_connection.State == ConnectionState.Open)
            {
                return;
            }
            _connection.Open();
        }

        public void Close()
        {
            if (_connection == null || _connection.State == ConnectionState.Closed || (InTransaction && !transaction.Ended))
            {
                return;
            }
            _connection.Close();
        }

        #endregion

        private void CreateConnection()
        {
            if (_connection == null)
            {
                _connection = _creator.Create();
            }
        }

        public void Dispose()
        {
            if (transaction != null)
            {
                transaction.Dispose();
                transaction = null;
            }

            if (_connection != null)
            {
                _connection.Dispose();
                _connection = null;
            }

            _creator = null;
        }
    }
}