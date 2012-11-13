using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using VODB.VirtualDataBase;

namespace VODB.DbLayer.DbCommands.DbParameterSetters
{
    public class ByteArrayParameterSetter : IParameterSetter
    {

        public Boolean CanHandle(Type type)
        {
            return typeof(Byte[]).IsAssignableFrom(type);
        }
        public void SetValue(DbParameter param, Field field, Object entity)
        {
            var value = field.GetValue(entity) as Byte[];

            param.Value = value;
            if (value == null)
            {
                param.Value = new Byte[] { };
                var type = param.DbType;
                param.Value = DBNull.Value;
                param.DbType = type;
            }
        }
    }
}
