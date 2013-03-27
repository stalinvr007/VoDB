namespace VODB.DbLayer
{
    
    /// <summary>
    /// This transaction can only rollback.
    /// </summary>
    class VodbInnerTransaction : IVodbTransaction
    {
        private readonly VodbTransaction _Transaction;

        public VodbInnerTransaction(VodbTransaction transaction)
        {
            _Transaction = transaction;
            _Transaction.BeginInnerTransaction();
        }

        public bool IsActive
        {
            get { return _Transaction.IsActive; }
        }

        public bool HasRolledBack { get; private set; }

        public void Commit()
        {
            if (HasRolledBack)
            {
                return;
            }
            
            HasRolledBack = true;
            _Transaction.EndInnerTransaction();
        }

        public void Rollback()
        {
            if (HasRolledBack)
            {
                return;
            }

            HasRolledBack = true;
            _Transaction.Rollback();
            _Transaction.EndInnerTransaction();
        }

        
    }
}
