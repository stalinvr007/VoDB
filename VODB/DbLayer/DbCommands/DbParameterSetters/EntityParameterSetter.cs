using System;
using System.Data.Common;
using System.Linq;
using VODB.Extensions;
using VODB.VirtualDataBase;

namespace VODB.DbLayer.DbCommands.DbParameterSetters
{
    public class EntityParameterSetter : IParameterSetter
    {
        #region IParameterSetter Members

        public Boolean CanHandle(Type type)
        {
            return EntityModelExtensions.IsEntity(type);
        }

        public void SetValue<TEntity>(DbParameter param, Field field, TEntity value)
        {
            var foreignEntity = value;

            if (foreignEntity == null)
            {
                param.Value = DBNull.Value;
            }
            else
            {
                var foreignKey = foreignEntity.GetTable<TEntity>().KeyFields
                    .FirstOrDefault(key => 
                        key.FieldName.Equals(field.BindedTo, StringComparison.InvariantCultureIgnoreCase) ||
                        key.FieldName.Equals(field.FieldName, StringComparison.InvariantCultureIgnoreCase));

                param.SetValue(foreignKey, foreignEntity);
            }
        }

        #endregion
    }
}