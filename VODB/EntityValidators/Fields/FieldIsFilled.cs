using System;
using VODB.Core.Infrastructure;

namespace VODB.EntityValidators.Fields
{
    public abstract class FieldIsFilled : IFieldValidator
    {
        #region IFieldValidator Members

        public Boolean CanHandle(Field field)
        {
            return CanHandle(field.FieldType);
        }

        public Boolean Verify<TEntity>(Field field, TEntity entity)
        {
            return IsFilled(field.GetValue(entity));
        }

        #endregion

        protected abstract Boolean IsFilled(object value);

        protected abstract Boolean CanHandle(Type fieldType);
    }
}