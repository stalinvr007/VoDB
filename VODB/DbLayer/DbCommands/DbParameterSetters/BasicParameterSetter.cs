using System;
using System.Data.Common;
using VODB.Core.Infrastructure;

namespace VODB.DbLayer.DbCommands.DbParameterSetters
{
    public class BasicParameterSetter : IParameterSetter
    {

        public void SetValue(DbParameter param, Field field, Object value)
        {
            param.Value = value ?? DBNull.Value;
        }

        public bool CanHandle(Type type)
        {
            return type.IsPrimitive || 
                typeof(String).IsAssignableFrom(type);
        }

    }
}
