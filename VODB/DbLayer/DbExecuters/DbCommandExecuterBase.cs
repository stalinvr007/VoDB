using System;
using System.Data.Common;
using VODB.DbLayer.DbCommands;
using VODB.Exceptions;

namespace VODB.DbLayer.DbExecuters
{
    internal abstract class DbCommandExecuterBase<TResult> : ICommandExecuter<TResult>
    {
        readonly IDbCommandFactory _commandFactory;

        protected DbCommandExecuterBase(IDbCommandFactory commandFactory)
        {
            _commandFactory = commandFactory;
        }

        public TResult Execute()
        {

            try
            {
                return Execute(_commandFactory.Make().ExecuteNonQuery());
            }
            catch (Exception ex)
            {
                    
                throw new UnableToExecuteNonQueryException(ex);
            }

        }

        protected abstract TResult Execute(int cmdResult);
    }
}