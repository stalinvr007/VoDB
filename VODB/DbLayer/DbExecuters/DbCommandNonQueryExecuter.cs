using VODB.DbLayer.DbCommands;

namespace VODB.DbLayer.DbExecuters
{
    internal sealed class DbCommandNonQueryExecuter : DbCommandExecuterBase<int>
    {
        public DbCommandNonQueryExecuter(IDbCommandFactory commandFactory)
            : base(commandFactory)
        {
        }

        protected override int Execute(int cmdResult)
        {
            return cmdResult;
        }
    }
}