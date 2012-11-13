using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using VODB.DbLayer;

namespace VODB
{
    
    /// <summary>
    /// Represents a connection Session.
    /// </summary>
    public sealed class Session : ISessionInternal, ISession 
    {

        private DbConnection _connection;
        private readonly IDbConnectionCreator _creator;
        private Transaction transaction;

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

            return _connection.CreateCommand();
        }

    }

    public sealed class Transaction : IDisposable
    {

        private int count = 1;
        private DbTransaction _Transaction;
        
        public Transaction(DbTransaction transaction)
        {
            _Transaction = transaction;
        }

        private void CheckTransactionAlive()
        {
            if (_Transaction == null)
            {
                throw new MissingFieldException("Transaction", "transaction");
            }
        }

        internal void BeginNestedTransaction()
        {
            CheckTransactionAlive();
            
            ++count;
        }

        public void RollBack()
        {
            CheckTransactionAlive();
            
            count = int.MaxValue;
            _Transaction.Rollback();
        }

        public void Commit()
        {
            CheckTransactionAlive();

            --count;
            if (count == 0)
            {
                _Transaction.Commit();
            }
        }

        public void Dispose()
        {
            if (_Transaction == null)
            {
                return;
            }

            Commit();
            if (count == 0)
            {
                _Transaction.Dispose();
                _Transaction = null;
            }
    
        }
    }

}
