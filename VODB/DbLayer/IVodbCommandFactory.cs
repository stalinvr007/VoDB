namespace VODB.DbLayer
{
    /// <summary>
    /// A Factory of IVodbCommand.
    /// </summary>
    public interface IVodbCommandFactory
    {

        /// <summary>
        /// Makes the command.
        /// </summary>
        /// <returns></returns>
        IVodbCommand MakeCommand();

    }
}
