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

        public void SetValue(DbParameter param, Field field, Object value)
        {
            var bytes = value as Byte[];

            param.Value = bytes;
            
            if (bytes != null) return;
            
            param.Value = new Byte[] {};
            var type = param.DbType;
            param.Value = DBNull.Value;
            param.DbType = type;
        }

        #endregion
    }
}