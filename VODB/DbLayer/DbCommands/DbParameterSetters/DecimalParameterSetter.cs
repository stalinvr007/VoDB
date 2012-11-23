using System;
using System.Data.Common;
using VODB.VirtualDataBase;

namespace VODB.DbLayer.DbCommands.DbParameterSetters
{
    public class DecimalParameterSetter : IParameterSetter
    {

        public Boolean CanHandle(Type type)
        {
            return typeof(Decimal).IsAssignableFrom(type);
        }

        public void SetValue(DbParameter param, Field field, Object entity)
        {
            param.Value = field.GetValue(entity) ?? DBNull.Value;
        }
    }
}
