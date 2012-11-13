using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using VODB.VirtualDataBase;

namespace VODB.DbLayer.DbCommands.DbParameterSetters
{
    public class BasicParameterSetter : IParameterSetter
    {

        public void SetValue(DbParameter param, Field field, Object entity)
        {
            param.Value = field.GetValue(entity);

            if (param.Value == null)
            {
                param.Value = DBNull.Value;
            }
        }

        public bool CanHandle(Type type)
        {
            return type.IsPrimitive || 
                typeof(String).IsAssignableFrom(type);
        }

    }
}
