using System;
using System.Linq;
using VODB.Extensions;

namespace VODB.EntityValidators.Fields
{
    public class DbEntityFieldIsFilled : FieldIsFilled
    {
        protected override Boolean IsFilled(object value)
        {
            var entity = value as DbEntity;
            return entity != null && entity.Table.KeyFields.All(entity.IsFilled);
        }

        protected override Boolean CanHandle(Type fieldType)
        {
            return typeof(DbEntity).IsAssignableFrom(fieldType);
        }
    }
}
