using System;
using System.Data.Common;
using VODB.Core.Infrastructure;

namespace VODB.Core.Execution.DbParameterSetters
{
    public class DateTimeParameterSetter : IParameterSetter
    {
        #region IParameterSetter Members

        public Boolean CanHandle(Type type)
        {
            return typeof (DateTime).IsAssignableFrom(type);
        }

        public void SetValue(DbParameter param, Field field, Object value)
        {
            var date = value as DateTime?;

            param.Value = (date == null || date.Value.Year == 1)
                              ? DBNull.Value
                              : (Object) date.Value;
        }

        #endregion
    }
}