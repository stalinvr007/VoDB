using System;
using System.Data;
using System.Data.Common;
using VODB.Core.Infrastructure;

namespace VODB.Core.Execution.DbParameterSetters
{
    public class DecimalParameterSetter : IParameterSetter
    {
        #region IParameterSetter Members

        public Boolean CanHandle(Type type)
        {
            return typeof (Decimal).IsAssignableFrom(type);
        }

        public void SetValue(DbParameter param, Field field, Object value)
        {
            param.Value = value ?? DBNull.Value;
        }

        #endregion
    }
}