using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VODB.Exceptions
{
    public class UnableToGetTheFirstMember : VodbException
    {
        public UnableToGetTheFirstMember(String msgMask, params Object[] args)
            : base(msgMask, args)
        {
            
        }
        public UnableToGetTheFirstMember(Exception innerException, String msgMask, params Object[] args)
            : base(innerException, msgMask, args)
        {
            
        }
    }
}
