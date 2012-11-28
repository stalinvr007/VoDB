using System;
using System.Collections.Generic;
using System.Data.Common;

namespace VODB.Sessions
{
    internal sealed class Transaction : ITransaction
    {
        private DbTransaction _Transaction;
        private int count = 1;

        public Boolean Ended { get; private set; }

        private LinkedList<String> _Savepoints = new LinkedList<String>();

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
            if (_Transaction.Connection == null)
            {
                throw new InvalidOperationException("Trying to create a Command withought a connection.");
            }

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
            SavePoint();
            ++count;
        }

        public void RollBack()
        {
            if (Ended)
            {
                return;
            }

            if (_Savepoints.Count > 0)
            {
                --count;
                var savepoint = _Savepoints.Last.Value;
                _Savepoints.RemoveLast();

                var trans = _Transaction as System.Data.SqlClient.SqlTransaction;
                trans.Rollback(savepoint);
                
                return;
            }

            CheckTransactionAlive();

            count = int.MaxValue;
            Ended = true;
            _Transaction.Rollback();
        }

        public void Commit()
        {
            if (Ended)
            {
                return;
            }

            if (_Savepoints.Count > 0)
            {
                _Savepoints.RemoveLast();
            }

            CheckTransactionAlive();

            --count;
            if (count == 0)
            {
                Ended = true;
                _Transaction.Commit();
            }
        }

        private void SavePoint()
        {
            if (typeof(System.Data.SqlClient.SqlTransaction) != _Transaction.GetType())
            {
                throw new NotSupportedException("Save points are available on MsSql connections.");
            }

            var trans = _Transaction as System.Data.SqlClient.SqlTransaction;

            string savepoint = String.Format("savepoint{0}", _Savepoints.Count);
            _Savepoints.AddLast(new LinkedListNode<String>(savepoint));
            trans.Save(savepoint);
        }

    }
}