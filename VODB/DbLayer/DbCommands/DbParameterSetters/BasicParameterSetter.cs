using System;
using System.Data.Common;
using VODB.VirtualDataBase;

namespace VODB.DbLayer.DbCommands.DbParameterSetters
{
    public class BasicParameterSetter : IParameterSetter
    {

        public void SetValue(DbParameter param, Field field, Object entity)
        {
            param.Value = field.GetValue(entity) ?? DBNull.Value;
        }

        public bool CanHandle(Type type)
        {
            return type.IsPrimitive || 
                typeof(String).IsAssignableFrom(type);
        }

    }
}
