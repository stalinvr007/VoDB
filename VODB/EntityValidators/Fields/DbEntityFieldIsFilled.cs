using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VODB.VirtualDataBase;
using VODB.Extensions;

namespace VODB.EntityValidators.Fields
{
    public class DbEntityFieldIsFilled : FieldIsFilled
    {
        protected override Boolean IsFilled(object value)
        {

            if (value == null)
            {
                return false;
            }

            var entity = value as DbEntity;
            foreach (var keyField in entity.Table.KeyFields)
            {
                if (!entity.IsFilled(keyField))
                {
                    return false;
                }
            }

            return true;
        }

        protected override Boolean CanHandle(Type fieldType)
        {
            return typeof(DbEntity).IsAssignableFrom(fieldType);
        }
    }
}
