using System;

namespace VODB.Exceptions
{
    public sealed class TruncatedException : VodbException
    {
        public TruncatedException(Exception ex)
            : base(ex, "There are fields that don't respect the field size limit.")
        { }
    }
}
