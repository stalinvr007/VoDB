using System;
using System.Data.Common;
using VODB.VirtualDataBase;

namespace VODB.DbLayer.DbCommands.DbParameterSetters
{
    public interface IParameterSetter
    {
        Boolean CanHandle(Type type);

        void SetValue(DbParameter param, Field field, Object value);
    }
}