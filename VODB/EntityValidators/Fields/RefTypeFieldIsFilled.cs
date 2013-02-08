using System;

namespace VODB.EntityValidators.Fields
{
    public class RefTypeFieldIsFilled : FieldIsFilled
    {
        protected override Boolean IsFilled(object value)
        {
            return value != null;
        }

        protected override Boolean CanHandle(Type fieldType)
        {
            return fieldType.IsClass;
        }
    }
}