using System;

namespace VODB.Exceptions
{
    public abstract class VodbException : Exception
    {

        public VodbException(String msgMask, params Object[] args)
            : base(String.Format(msgMask, args))
        { }

    }
}
