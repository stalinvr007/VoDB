using System;

namespace VODB.EntityValidators.Fields
{
    internal class StringFieldIsFilled : FieldIsFilled
    {
        protected override Boolean IsFilled(object value)
        {
            return value != null && !String.IsNullOrEmpty(value.ToString());
        }

        protected override Boolean CanHandle(Type fieldType)
        {
            return fieldType == typeof (String);
        }
    }
}