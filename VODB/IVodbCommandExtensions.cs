using System.Collections.Generic;
using VODB.DbLayer;
using VODB.ExpressionsToSql;

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

        public static IVodbCommand SetParameters(this IVodbCommand command, IEnumerable<IQueryParameter> param)
        {
            foreach (var item in param)
            {
                command.AddParameter(item);
            }
            return command;
        }

        public static IVodbCommand SetParametersNames(this IVodbCommand command, IEnumerable<string> names)
        {
            command.CreateParameters(names);
            return command;
        }

        public static IVodbCommand SetParametersValues(this IVodbCommand command, IEnumerable<IQueryParameter> parameters)
        {
            command.RefreshParametersValues(parameters);
            return command;
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
