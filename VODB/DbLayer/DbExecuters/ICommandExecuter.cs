namespace VODB.DbLayer.DbExecuters
{
    /// <summary>
    /// This interface represents a Command to be executed against the database.
    /// </summary>
    interface ICommandExecuter<out TResult>
    {

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <returns></returns>
        TResult Execute();

    }
}
