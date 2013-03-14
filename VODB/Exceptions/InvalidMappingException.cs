using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VODB.Exceptions
{
    public class InvalidMappingException : VodbException
    {
        public InvalidMappingException(String msgMask, params Object[] args)
            : base(msgMask, args)
        {
            
        }
        public InvalidMappingException(Exception innerException, String msgMask, params Object[] args)
            : base(innerException, msgMask, args)
        {
            
        }
                                                                  
    }
}
