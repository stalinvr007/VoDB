using System;

namespace VODB.Exceptions
{
    public class UnableToGetTheValue : VodbException
    {
        public UnableToGetTheValue(String msgMask, params Object[] args)
            : base(msgMask, args)
        {
        }

        public UnableToGetTheValue(Exception innerException, String msgMask, params Object[] args)
            : base(innerException, msgMask, args)
        {
        }
    }
}