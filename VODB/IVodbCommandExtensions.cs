using VODB.DbLayer;

namespace VODB
{
    internal static class IVodbCommandExtensions
    {
        public static IVodbCommand MakeCommand(this IVodbCommandFactory connection, string sqlCmd)
        {
            var cmd = connection.MakeCommand();
            cmd.SetCommandText(sqlCmd);
            return cmd;
        }

        public static TResult ExecuteScalar<TResult>(this IVodbCommandFactory commandFactory, string sqlCmd)
        {
            return (TResult)commandFactory.MakeCommand(sqlCmd).ExecuteScalar();
        }

        public static int Execute(this IVodbCommandFactory commandFactory, string sqlCmd)
        {
            return commandFactory.MakeCommand(sqlCmd).ExecuteNonQuery();
        }
    }
}
