using System;
using System.Linq;

namespace VODB.EntityValidators.Fields
{
    public class DbEntityFieldIsFilled : FieldIsFilled
    {
        protected override Boolean IsFilled(object value)
        {
            return value.GetTable().KeyFields.Any();
        }

        protected override Boolean CanHandle(Type fieldType)
        {
            return fieldType.IsEntity();
        }
    }
}