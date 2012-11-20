using System;

namespace VODB.Exceptions
{
    public sealed class UnableToExecuteNonQueryException : VodbException
    {
        public UnableToExecuteNonQueryException(Exception exception)
            : base("", exception)
        { }
    }
}