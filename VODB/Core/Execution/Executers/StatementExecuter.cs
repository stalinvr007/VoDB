using System.Data;
using System.Data.Common;

namespace VODB.Core.Execution.Executers
{
    internal class StatementExecuter : IStatementExecuter
    {
        #region Implementation of IStatementExecuter

        public void Execute(string statement, IInternalSession session)
        {
            DbCommand command = session.CreateCommand();
            command.CommandText = statement;

            command.ExecuteNonQuery();
        }

        #endregion
    }
}