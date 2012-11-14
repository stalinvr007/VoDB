using System;
using System.Data.Common;

namespace VODB
{
    public sealed class Transaction : IDisposable
    {
        private DbTransaction _Transaction;
        private int count = 1;

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
            if (count == 0)
            {
                _Transaction.Dispose();
                _Transaction = null;
            }
        }

        #endregion

        internal DbCommand CreateCommand()
        {
            DbCommand cmd = _Transaction.Connection.CreateCommand();
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
    }
}