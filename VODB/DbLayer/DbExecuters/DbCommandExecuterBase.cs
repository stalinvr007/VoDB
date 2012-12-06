using System;
using VODB.DbLayer.DbCommands;
using VODB.Extensions;

namespace VODB.DbLayer.DbExecuters
{
    internal abstract class DbCommandExecuterBase<TResult> : ICommandExecuter<TResult>
    {
        private readonly IDbCommandFactory _commandFactory;

        protected DbCommandExecuterBase(IDbCommandFactory commandFactory)
        {
            _commandFactory = commandFactory;
        }

        #region ICommandExecuter<TResult> Members

        public TResult Execute()
        {
            try
            {
                return Execute(_commandFactory.Make().ExecuteNonQuery());
            }
            catch (Exception ex)
            {
                ex.HandleException();
            }
            return default(TResult);
        }

        #endregion

        protected abstract TResult Execute(int cmdResult);
    }
}