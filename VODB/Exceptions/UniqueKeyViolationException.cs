using System;

namespace VODB.Exceptions
{
    public sealed class UniqueKeyViolationException : VodbException
    {
        public UniqueKeyViolationException(Exception ex)
            : base(ex, "The execution of a command has resulted in a Unique Key Violation.")
        { }
    }
}