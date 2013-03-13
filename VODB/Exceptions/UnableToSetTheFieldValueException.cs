using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VODB.Exceptions
{
    public class UnableToSetTheFieldValueException : VodbException
    {
        private const string NOT_NULL_VALUE_MSG = "Its not possible to set the field [{0}] with the value [{1}] of type [{2}]";
        private const string NULL_VALUE_MSG = "Its not possible to set the field [{0}]";

        public UnableToSetTheFieldValueException(Exception ex, string fieldName, object value)
            : base(ex, 
            value != null ? NOT_NULL_VALUE_MSG : NULL_VALUE_MSG, 
            fieldName, value, value != null ? value.GetType() : null) { }

        public UnableToSetTheFieldValueException(Exception ex, string fieldName)
            : base(ex, NULL_VALUE_MSG , fieldName) { }
    }
}
