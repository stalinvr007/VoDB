using System;

namespace VODB.Exceptions
{
    public class UnableToExecuteQueryException : VodbException
    {
        public UnableToExecuteQueryException(string commandText, Exception ex)
            : base(ex, "An error occurred when executing the following query:\n{0}", commandText)
        {
        }
    }
}