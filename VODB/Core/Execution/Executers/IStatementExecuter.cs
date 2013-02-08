namespace VODB.Core.Execution.Executers
{
    /// <summary>
    /// Represents a StatementExecuter.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    internal interface IStatementExecuter<out TResult>
    {
        /// <summary>
        /// Executes a command using the specified entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="session">The session.</param>
        /// <returns></returns>
        TResult Execute<TEntity>(TEntity entity, IInternalSession session);
    }

    /// <summary>
    /// Represents a StatementExecuter
    /// </summary>
    internal interface IStatementExecuter
    {
        /// <summary>
        /// Executes the specified statement.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <param name="session">The session.</param>
        void Execute(string statement, IInternalSession session);
    }
}