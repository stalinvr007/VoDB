using System;
using VODB.Core.Infrastructure;

namespace VODB.EntityValidators.Fields
{
    public abstract class FieldIsFilled : IFieldValidator
    {
        public Boolean CanHandle(Field field)
        {
            return CanHandle(field.FieldType);
        }

        public Boolean Verify(Field field, Entity entity)
        {
            return IsFilled(field.GetValue(entity));
        }

        protected abstract Boolean IsFilled(object value);

        protected abstract Boolean CanHandle(Type fieldType);
    }
}
