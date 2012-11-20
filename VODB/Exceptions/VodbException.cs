using System;

namespace VODB.Exceptions
{
    public abstract class VodbException : Exception
    {

        public VodbException(String msgMask, params Object[] args)
            : base(Format(msgMask, args))
        { }

        private static string Format(string msgMask, object[] args)
        {
            return args == null || args.Length == 0 ? msgMask : String.Format(msgMask, args);
        }

    }
}
