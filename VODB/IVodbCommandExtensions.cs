using VODB.DbLayer;

namespace VODB
{
    internal static class IVodbCommandExtensions
    {
        public static IVodbCommand MakeCommand(this IDbCommandFactory connection, string sqlCmd)
        {
            var cmd = connection.MakeCommand();
            cmd.SetCommandText(sqlCmd);
            return cmd;
        }

        public static TResult ExecuteScalar<TResult>(this IDbCommandFactory commandFactory, string sqlCmd)
        {
            return (TResult)commandFactory.MakeCommand(sqlCmd).ExecuteScalar();
        }

        public static int Execute(this IDbCommandFactory commandFactory, string sqlCmd)
        {
            return commandFactory.MakeCommand(sqlCmd).ExecuteNonQuery();
        }
    }
}
