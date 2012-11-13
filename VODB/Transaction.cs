using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using VODB.DbLayer;

namespace VODB
{
    public sealed class Transaction : IDisposable
    {

        private int count = 1;
        private DbTransaction _Transaction;

        public Transaction(DbTransaction transaction)
        {
            _Transaction = transaction;
        }

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
