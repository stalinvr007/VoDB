namespace VODB.DbLayer
{
    /// <summary>
    /// Represents a scope of execution.
    /// </summary>
    public interface IVodbTransaction : ITransaction
    {

        /// <summary>
        /// Gets a value indicating whether this instance has inner transactions.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has inner transactions; otherwise, <c>false</c>.
        /// </value>
        bool IsActive { get; }

        
    }
}
