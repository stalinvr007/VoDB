using System;
using System.Data.Common;
using System.Linq;
using VODB.Extensions;
using VODB.Core.Infrastructure;

namespace VODB.Core.Execution.DbParameterSetters
{
    public class DbEntityParameterSetter : IParameterSetter
    {
        #region IParameterSetter Members

        public Boolean CanHandle(Type type)
        {
            return type.IsEntity();
        }

        public void SetValue(DbParameter param, Field field, Object value)
        {
            var foreignEntity = value;

            if (foreignEntity == null)
            {
                param.Value = DBNull.Value;
            }
            else
            {
                var foreignKey = foreignEntity.GetTable().KeyFields
                    .FirstOrDefault(key => 
                        key.FieldName.Equals(field.BindedTo, StringComparison.InvariantCultureIgnoreCase) ||
                        key.FieldName.Equals(field.FieldName, StringComparison.InvariantCultureIgnoreCase));

                param.SetValue(foreignKey, foreignEntity);
            }
        }

        #endregion
    }
}