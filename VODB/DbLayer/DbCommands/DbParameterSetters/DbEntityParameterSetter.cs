using System;
using System.Data.Common;
using System.Linq;
using VODB.Extensions;
using VODB.VirtualDataBase;

namespace VODB.DbLayer.DbCommands.DbParameterSetters
{
    public class DbEntityParameterSetter : IParameterSetter
    {
        #region IParameterSetter Members

        public Boolean CanHandle(Type type)
        {
            return typeof (DbEntity).IsAssignableFrom(type);
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
                Field foreignKey = foreignEntity.Table.KeyFields
                    .FirstOrDefault(key => key.FieldName.Equals(field.BindedTo) || key.FieldName.Equals(field.FieldName));

                param.SetValue(foreignKey, foreignEntity);
            }
        }

        #endregion
    }
}