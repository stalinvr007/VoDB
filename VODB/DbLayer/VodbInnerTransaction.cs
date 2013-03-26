namespace VODB.DbLayer
{
    
    /// <summary>
    /// This transaction can only rollback.
    /// </summary>
    class VodbInnerTransaction : IVodbTransaction
    {
        private readonly IVodbTransaction _Transaction;

        public VodbInnerTransaction(IVodbTransaction transaction)
        {
            _Transaction = transaction;
        }

        public bool HasRolledBack
        {
            get { return _Transaction.HasRolledBack; }
        }

        public void Commit() { /* */ }

        public void Rollback()
        {
            _Transaction.Rollback();
        }

        
    }
}
