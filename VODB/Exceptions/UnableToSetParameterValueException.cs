using System;

namespace VODB.Exceptions
{
    public class UnableToSetParameterValueException : VodbException
    {
        public UnableToSetParameterValueException(Exception ex, string tableName, VirtualDataBase.Field field, object value)
            : base(ex, "Unable to set the field [{1}] of [{0}] with the value [{2}].", tableName, field.FieldName, value)
        { }
    }
}
