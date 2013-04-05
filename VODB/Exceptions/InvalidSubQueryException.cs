using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VODB.Exceptions
{
    public class InvalidSubQueryException : VodbException
    {
        public InvalidSubQueryException(String msgMask, params Object[] args)
            : base(msgMask, args)
        {
            
        }
        public InvalidSubQueryException(Exception innerException, String msgMask, params Object[] args)
            : base(innerException, msgMask, args)
        {
            
        }
    }
}
