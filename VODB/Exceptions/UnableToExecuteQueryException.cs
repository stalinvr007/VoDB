using System;

namespace VODB.Exceptions
{
    public class UnableToExecuteQueryException : Exception
    {
        public UnableToExecuteQueryException(string commandText, Exception ex)
            : base(String.Format("An error occurred when executing the following query:\n{0}", commandText), ex)
        {
        }
    }
}