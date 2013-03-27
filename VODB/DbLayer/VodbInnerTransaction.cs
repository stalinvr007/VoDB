namespace VODB.DbLayer
{
    
    /// <summary>
    /// This transaction can only rollback.
    /// </summary>
    class VodbInnerTransaction : IVodbTransaction
    {
        private VodbTransaction _Transaction;

        public VodbInnerTransaction(VodbTransaction transaction)
        {
            _Transaction = transaction;
            _Transaction.BeginInnerTransaction();
        }

        public bool IsActive
        {
            get { return _Transaction.IsActive; }
        }

        public bool RolledBack { get; private set; }

        public void Commit()
        {
            if (RolledBack)
            {
                return;
            }
            
            RolledBack = true;
            _Transaction.EndInnerTransaction();
        }

        public void Rollback()
        {
            if (RolledBack)
            {
                return;
            }

            RolledBack = true;
            _Transaction.Rollback();
            _Transaction.EndInnerTransaction();
        }

        public void Dispose()
        {
            _Transaction = null;
        }
    }
}
