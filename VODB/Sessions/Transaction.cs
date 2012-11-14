using System;
using System.Data.Common;

namespace VODB.Sessions
{
    internal sealed class Transaction : IDisposable, ITransaction
    {
        private DbTransaction _Transaction;
        private int count = 1;

        public Boolean Ended { get; private set; }

        public Transaction(DbTransaction transaction)
        {
            _Transaction = transaction;
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (_Transaction == null)
            {
                return;
            }

            Commit();
            
            if (count != 0) return;

            _Transaction.Dispose();
            _Transaction = null;
        }

        #endregion

        internal DbCommand CreateCommand()
        {
            var cmd = _Transaction.Connection.CreateCommand();
            cmd.Transaction = _Transaction;
            return cmd;
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
            Ended = true;
            _Transaction.Rollback();
        }

        public void Commit()
        {
            CheckTransactionAlive();

            --count;
            if (count == 0)
            {
                Ended = true;
                _Transaction.Commit();
            }
        }
    }
}