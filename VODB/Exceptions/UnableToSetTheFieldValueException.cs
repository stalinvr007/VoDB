using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VODB.Exceptions
{
    public class UnableToSetTheFieldValueException : VodbException
    {
        public UnableToSetTheFieldValueException(Exception ex, string fieldName, object value)
            : base(ex, "Its not possible to set the field [{0}] with the value [{1}] of type [{2}]", fieldName, value, value.GetType())
        { }
    }
}
