using System.Data.Common;

namespace VODB.DbLayer
{

    class VodbTransaction : IVodbTransaction
    {
        private readonly DbTransaction _Transaction;
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

        public bool HasRolledBack { get; private set; }

        public void Commit()
        {
            if (HasRolledBack)
            {
                return;
            }

            _Transaction.Commit();
            EndTransaction();
        }

        public void Rollback()
        {
            if (HasRolledBack)
            {
                return;
            }

            _Transaction.Rollback();
            HasRolledBack = true;
            EndTransaction();
        }

        private void EndTransaction()
        {
            innerTransactionCount = 0;
        }
    }
}
