using System;

namespace VODB.EntityValidators.Fields
{
    internal class DateTimeFieldIsFilled : FieldIsFilled
    {

        protected override Boolean IsFilled(object value)
        {
            return ((DateTime)value).Year > 1974;
        }
        protected override Boolean CanHandle(Type fieldType)
        {
            return typeof(DateTime).IsAssignableFrom(fieldType);
        }
    }
}
