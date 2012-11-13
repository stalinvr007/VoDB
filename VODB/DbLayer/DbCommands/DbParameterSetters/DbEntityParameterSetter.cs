using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using VODB.VirtualDataBase;
using VODB.Extensions;

namespace VODB.DbLayer.DbCommands.DbParameterSetters
{
    public class DbEntityParameterSetter : IParameterSetter
    {

        public Boolean CanHandle(Type type)
        {
            return typeof(DbEntity).IsAssignableFrom(type);
        }

        public void SetValue(DbParameter param, Field field, Object entity)
        {

            var foreignEntity = field.GetValue(entity) as DbEntity;

            if (foreignEntity == null)
            {
                param.Value = DBNull.Value;
            }
            else
            {
                var foreignKey = foreignEntity.Table.KeyFields
                       .FirstOrDefault(key => key.Equals(field.BindedTo) || key.Equals(field.FieldName));

                param.SetValue(foreignKey, foreignEntity);
            }

        }

    }
}
