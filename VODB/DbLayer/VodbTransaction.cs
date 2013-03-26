using System.Data.Common;

namespace VODB.DbLayer
{

    class VodbTransaction : IVodbTransaction
    {
        private readonly DbTransaction _Transaction;

        public VodbTransaction(DbTransaction transaction)
        {
            _Transaction = transaction;
        }

        public bool HasRolledBack { get; private set; }

        public void Commit()
        {
            if (HasRolledBack)
            {
                return;
            }

            _Transaction.Commit();
        }

        public void Rollback()
        {
            if (HasRolledBack)
            {
                return;
            }

            _Transaction.Rollback();
            HasRolledBack = true;
        }
    }
}
