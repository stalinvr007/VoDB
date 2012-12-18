using System;
using System.Data.Common;
using VODB.Infrastructure;

namespace VODB.DbLayer.DbCommands.DbParameterSetters
{
    public class GuidParameterSetter : IParameterSetter
    {

        public void SetValue(DbParameter param, Field field, Object value)
        {
            param.Value = value ?? DBNull.Value;
        }

        public bool CanHandle(Type type)
        {
            return typeof(Guid).IsAssignableFrom(type);
        }

    }
}