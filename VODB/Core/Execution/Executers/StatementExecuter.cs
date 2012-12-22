namespace VODB.Core.Execution.Executers
{
    class StatementExecuter : IStatementExecuter
    {
        #region Implementation of IStatementExecuter

        public void Execute(string statement, IInternalSession session)
        {
            var command = session.CreateCommand();
            command.CommandText = statement;

            command.ExecuteNonQuery();
        }

        #endregion
    }
}