using System;
using System.Collections.Generic;
using System.Data.Common;

namespace VODB.Sessions
{
    interface IInternalTransaction : ITransaction
    {
        ITransaction BeginTransaction(IInternalSession session, DbConnection connection);
        DbCommand CreateCommand();
        Boolean Ended { get; }
    }

    internal sealed class Transaction : IInternalTransaction
    {
        private DbTransaction _Transaction;

        public Boolean Ended { get; private set; }

        public Boolean RolledBack { get; private set; }

        private readonly LinkedList<String> _Savepoints = new LinkedList<String>();
        private IInternalSession _Session;

        #region IDisposable Members

        public void Dispose()
        {
            if (_Transaction == null)
            {
                return;
            }

            if (!RolledBack)
            {
                Commit();
            }

            _Transaction.Dispose();
            _Transaction = null;
        }

        #endregion

        public Transaction()
        {
            Ended = true;
        }

        private void CheckTransactionAlive()
        {
            if (_Transaction == null)
            {
                throw new MissingFieldException("Transaction", "transaction");
            }
        }

        public void RollBack()
        {
            if (Ended)
            {
                return;
            }

            if (!RolledBack)
            {
                RolledBack = true;

                if (_Savepoints.Count > 0)
                {
                    var savepoint = _Savepoints.Last.Value;
                    _Savepoints.RemoveLast();

                    var trans = _Transaction as System.Data.SqlClient.SqlTransaction;
                    if (trans != null)
                    {
                        trans.Rollback(savepoint);
                    }

                    return;
                }
            }

            CheckTransactionAlive();

            Ended = true;
            _Transaction.Rollback();
            _Session.Close();
            _Transaction = null;
        }

        public void Commit()
        {
            if (Ended || RolledBack)
            {
                return;
            }

            if (_Savepoints.Count > 0)
            {
                _Savepoints.RemoveLast();
                return;
            }

            CheckTransactionAlive();

            Ended = true;
            if (!RolledBack)
            {
                _Transaction.Commit();
                _Session.Close();
            }

            _Transaction = null;
        }

        private void SavePoint()
        {
            if (typeof(System.Data.SqlClient.SqlTransaction) != _Transaction.GetType())
            {
                throw new NotSupportedException("Save points are available on MsSql connections.");
            }

            var trans = _Transaction as System.Data.SqlClient.SqlTransaction;

            var savepoint = String.Format("savepoint{0}", _Savepoints.Count);
            _Savepoints.AddLast(new LinkedListNode<String>(savepoint));
            if (trans != null)
            {
                trans.Save(savepoint);
            }
        }

        public ITransaction BeginTransaction(IInternalSession session, DbConnection connection)
        {
            _Session = session;
            if (_Transaction == null)
            {
                Ended = false;
                _Transaction = connection.BeginTransaction();
            }
            else
            {
                BeginNestedTransaction();
            }
            return this;
        }

        public DbCommand CreateCommand()
        {
            if (_Transaction.Connection == null)
            {
                throw new InvalidOperationException("Trying to create a Command withought a connection.");
            }

            var cmd = _Transaction.Connection.CreateCommand();
            cmd.Transaction = _Transaction;
            return cmd;
        }

        internal void BeginNestedTransaction()
        {
            CheckTransactionAlive();
            SavePoint();
        }

    }

}