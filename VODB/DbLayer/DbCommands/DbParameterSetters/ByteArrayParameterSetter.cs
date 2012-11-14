using System;
using System.Data;
using System.Data.Common;
using VODB.VirtualDataBase;

namespace VODB.DbLayer.DbCommands.DbParameterSetters
{
    public class ByteArrayParameterSetter : IParameterSetter
    {
        #region IParameterSetter Members

        public Boolean CanHandle(Type type)
        {
            return typeof (Byte[]).IsAssignableFrom(type);
        }

        public void SetValue(DbParameter param, Field field, Object entity)
        {
            var value = field.GetValue(entity) as Byte[];

            param.Value = value;
            if (value == null)
            {
                param.Value = new Byte[] {};
                DbType type = param.DbType;
                param.Value = DBNull.Value;
                param.DbType = type;
            }
        }

        #endregion
    }
}