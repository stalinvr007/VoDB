namespace VODB.DbLayer
{
    /// <summary>
    /// Represents a scope of execution.
    /// </summary>
    public interface IVodbTransaction
    {
        /// <summary>
        /// Gets a value indicating whether this instance has rolled back.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has rolled back; otherwise, <c>false</c>.
        /// </value>
        bool HasRolledBack { get; }

        /// <summary>
        /// Commits the changes made within this scope to the database.
        /// </summary>
        void Commit();

        /// <summary>
        /// Rollbacks all changes made within this scope.
        /// </summary>
        void Rollback();
    }
}
