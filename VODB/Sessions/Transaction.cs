using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;

namespace VODB.Sessions
{
    internal interface IInternalTransaction : ITransaction
    {
        Boolean Ended { get; }
        ITransaction BeginTransaction(IInternalSession session, DbConnection connection);
        DbCommand CreateCommand();
    }

    internal sealed class Transaction : IInternalTransaction
    {
        private readonly LinkedList<String> _Savepoints = new LinkedList<String>();
        private IInternalSession _Session;
        private DbTransaction _Transaction;

        public Transaction()
        {
            Ended = true;
        }

        #region IInternalTransaction Members

        public Boolean Ended { get; private set; }

        public Boolean RolledBack { get; private set; }

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
                    string savepoint = _Savepoints.Last.Value;
                    _Savepoints.RemoveLast();

                    var trans = _Transaction as SqlTransaction;
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

            DbCommand cmd = _Transaction.Connection.CreateCommand();
            cmd.Transaction = _Transaction;
            return cmd;
        }

        #endregion

        private void CheckTransactionAlive()
        {
            if (_Transaction == null)
            {
                throw new MissingFieldException("Transaction", "transaction");
            }
        }

        private void SavePoint()
        {
            if (typeof (SqlTransaction) != _Transaction.GetType())
            {
                throw new NotSupportedException("Save points are available on MsSql connections.");
            }

            var trans = _Transaction as SqlTransaction;

            string savepoint = String.Format("savepoint{0}", _Savepoints.Count);
            _Savepoints.AddLast(new LinkedListNode<String>(savepoint));
            if (trans != null)
            {
                trans.Save(savepoint);
            }
        }

        internal void BeginNestedTransaction()
        {
            CheckTransactionAlive();
            SavePoint();
        }
    }
}