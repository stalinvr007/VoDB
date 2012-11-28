using System;

namespace VODB.Exceptions
{
    public sealed class PrimaryKeyViolationException : VodbException
    {
        public PrimaryKeyViolationException(Exception ex)
            : base(ex, "The execution of a command has resulted in a Primary Key Violation.")
        { }
    }
}