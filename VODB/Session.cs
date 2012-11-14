using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using VODB.DbLayer;

namespace VODB
{
    /// <summary>
    /// Represents a connection Session.
    /// </summary>
    public abstract class Session : ISessionInternal, ISession
    {
        private readonly IDbConnectionCreator _creator;
        private DbConnection _connection;
        private Transaction transaction;


        /// <summary>
        /// Initializes a new instance of the <see cref="Session" /> class.
        /// </summary>
        /// <param name="creator">The creator.</param>
        internal Session(IDbConnectionCreator creator = null)
        {
            _creator = creator ?? new SqlConnectionCreator();
        }

        internal bool InTransaction
        {
            get { return transaction != null; }
        }

        #region ISession Members

        public Transaction BeginTransaction()
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

        #region ISessionInternal Members

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
            if (_connection == null || _connection.State == ConnectionState.Closed)
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