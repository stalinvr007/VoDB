using System;
using System.Data.Common;
using VODB.Exceptions;

namespace VODB.DbLayer.DbExecuters
{
    internal abstract class DbCommandExecuterBase<TResult> : ICommandExecuter<TResult>
    {
        private readonly DbCommand _command;

        protected DbCommandExecuterBase(DbCommand command)
        {
            _command = command;
        }

        public TResult Execute()
        {

            try
            {
                return Execute(_command.ExecuteNonQuery());
            }
            catch (Exception ex)
            {
                    
                throw new UnableToExecuteNonQueryException(ex);
            }

        }

        protected abstract TResult Execute(int cmdResult);
    }
}