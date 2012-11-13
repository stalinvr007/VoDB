using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using VODB.DbLayer;
using VODB.DbLayer.DbCommands;
using VODB.DbLayer.DbExecuters;
using VODB.DbLayer.Loaders;

namespace VODB
{

    /// <summary>
    /// Represents a connection Session.
    /// </summary>
    public abstract class Session : ISessionInternal, ISession
    {

        private DbConnection _connection;
        private readonly IDbConnectionCreator _creator;
        private Transaction transaction;


        internal bool InTransaction
        {
            get
            {
                return transaction != null;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Session" /> class.
        /// </summary>
        /// <param name="creator">The creator.</param>
        internal Session(IDbConnectionCreator creator = null)
        {
            _creator = creator;

            if (_creator == null)
            {
                _creator = new SqlConnectionCreator();
            }
        }

        private void CreateConnection()
        {
            if (_connection == null)
            {
                _connection = _creator.Create();
            }
        }

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

        public DbCommand CreateCommand()
        {
            CreateConnection();

            return InTransaction ? 
                transaction.CreateCommand() : 
                _connection.CreateCommand();
        }



        public void Open()
        {
            CreateCommand();

            if (_connection.State == System.Data.ConnectionState.Open)
            {
                return;
            }
            _connection.Open();
        }

        public void Close()
        {
            if (_connection == null || _connection.State == System.Data.ConnectionState.Closed)
            {
                return;
            }
            _connection.Close();
        }
        
        public abstract IEnumerable<TEntity> GetAll<TEntity>() where TEntity : DbEntity, new();

    }

}
