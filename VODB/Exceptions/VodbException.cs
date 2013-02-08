using System;

namespace VODB.Exceptions
{
    public abstract class VodbException : Exception
    {
        protected VodbException(String msgMask, params Object[] args)
            : base(Format(msgMask, args))
        {
        }

        protected VodbException(Exception innerException, String msgMask, params Object[] args)
            : base(Format(msgMask, args), innerException)
        {
        }

        private static string Format(string msgMask, object[] args)
        {
            return args == null || args.Length == 0 ? msgMask : String.Format(msgMask, args);
        }
    }
}