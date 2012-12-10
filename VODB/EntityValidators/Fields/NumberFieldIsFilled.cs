using System;

namespace VODB.EntityValidators.Fields
{
    internal class NumberFieldIsFilled : FieldIsFilled
    {

        protected override Boolean IsFilled(object value)
        {
            return !value.Equals(0);
        }
        protected override Boolean CanHandle(Type fieldType)
        {
            return typeof(int).IsAssignableFrom(fieldType) ||
                typeof(Double).IsAssignableFrom(fieldType) ||
                typeof(float).IsAssignableFrom(fieldType) ||
                typeof(Decimal).IsAssignableFrom(fieldType) ||
                typeof(long).IsAssignableFrom(fieldType);

        }
    }
}
