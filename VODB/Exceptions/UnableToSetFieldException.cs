using System;
using VODB.VirtualDataBase;

namespace VODB.Exceptions
{
    public class UnableToSetFieldException : VodbException
    {
        
        public UnableToSetFieldException(Exception ex, String tableName, Field field, object value)
            :base(ex, "Unable to set the field [{1}] of [{0}] with the value [{2}]", tableName, field.FieldName, value)
        { }

    }
}