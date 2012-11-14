using System;
using System.Data.Common;
using VODB.VirtualDataBase;

namespace VODB.DbLayer.DbCommands.DbParameterSetters
{
    public class DateTimeParameterSetter : IParameterSetter
    {
        #region IParameterSetter Members

        public Boolean CanHandle(Type type)
        {
            return typeof (DateTime).IsAssignableFrom(type);
        }

        public void SetValue(DbParameter param, Field field, Object entity)
        {
            var value = field.GetValue(entity) as DateTime?;

            param.Value = (value == null || value.Value.Year == 1)
                              ? DBNull.Value
                              : (Object) value.Value;
        }

        #endregion
    }
}