using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VODB.Exceptions
{
    public class OrderByClauseException : VodbException
    {
        public OrderByClauseException(String msgMask, params Object[] args)
            : base(msgMask, args)
        {
            
        }
        public OrderByClauseException(Exception innerException, String msgMask, params Object[] args)
            : base(innerException, msgMask, args)
        {
            
        }
    }
}
