using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VODB.VirtualDataBase;
using VODB.Extensions;

namespace VODB.EntityValidators.Fields
{
    public abstract class FieldIsFilled : IFieldValidator
    {
        public Boolean CanHandle(Field field)
        {
            return CanHandle(field.FieldType);
        }

        public Boolean Verify(Field field, DbEntity entity)
        {
            return IsFilled(field.GetValue(entity));
        }

        protected abstract Boolean IsFilled(object value);

        protected abstract Boolean CanHandle(Type fieldType);
    }
}
