using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VODB.Exceptions
{
    public class UnableToGetTheFieldValueException : VodbException
    {
        public UnableToGetTheFieldValueException(Exception ex, string fieldName)
            : base(ex, "Unable to get the value of field [{0}].", fieldName) { }
         
    }
}
