using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using VODB.DbLayer;

namespace VODB.Sessions
{
    /// <summary>
    /// Represents a connection internalSession.
    /// </summary>
    internal abstract class InternalSession : IInternalSession, ISession
    {
        private readonly IDbConnectionCreator _creator;
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
    }
}