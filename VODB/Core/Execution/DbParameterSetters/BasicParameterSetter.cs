using System;
using System.Data;
using System.Data.Common;
using VODB.Core.Infrastructure;

namespace VODB.Core.Execution.DbParameterSetters
{
    public class BasicParameterSetter : IParameterSetter
    {
        #region IParameterSetter Members

        public void SetValue(DbParameter param, Field field, Object value)
        {
            param.Value = value ?? DBNull.Value;
        }

        public bool CanHandle(Type type)
        {
            return type.IsPrimitive ||
                   typeof (String).IsAssignableFrom(type);
        }

        #endregion
    }
}