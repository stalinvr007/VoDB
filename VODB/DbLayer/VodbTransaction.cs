using System.Data.Common;

namespace VODB.DbLayer
{

    class VodbTransaction : IVodbTransaction
    {
        private DbTransaction _Transaction;
        private int innerTransactionCount = 1;

        public VodbTransaction(DbTransaction transaction)
        {
            _Transaction = transaction;
        }

        public void BeginInnerTransaction()
        {
            ++innerTransactionCount;
        }

        public void EndInnerTransaction()
        {
            --innerTransactionCount;
        }

        public bool IsActive
        {
            get { return innerTransactionCount > 0; }
        }

        public bool RolledBack { get; private set; }

        public void Commit()
        {
            if (RolledBack)
            {
                return;
            }

            _Transaction.Commit();
            EndTransaction();
        }

        public void Rollback()
        {
            if (RolledBack)
            {
                return;
            }

            _Transaction.Rollback();
            RolledBack = true;
            EndTransaction();
        }

        private void EndTransaction()
        {
            innerTransactionCount = 0;
        }

        public void Dispose()
        {
            if (_Transaction == null)
            {
                return;
            }

            _Transaction.Dispose();
            _Transaction = null;
        }
    }
}
