using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using VODB.VirtualDataBase;

namespace VODB.DbLayer.DbCommands.DbParameterSetters
{
    public class DateTimeParameterSetter : IParameterSetter
    {
        
        public Boolean CanHandle(Type type)
        {
            return typeof(DateTime).IsAssignableFrom(type);
        }
        public void SetValue(DbParameter param, Field field, Object entity)
        {
            var value = field.GetValue(entity) as DateTime?;

            param.Value = (value == null || value.Value.Year == 1) ? 
                DBNull.Value : 
                (Object)value.Value;

        }
    }
}
