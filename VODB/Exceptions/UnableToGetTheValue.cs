using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
