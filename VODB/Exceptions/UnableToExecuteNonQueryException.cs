using System;

namespace VODB.Exceptions
{
    public sealed class UnableToExecuteNonQueryException : Exception
    {
        public UnableToExecuteNonQueryException(Exception exception)
            : base("", exception)
        { }
    }
}