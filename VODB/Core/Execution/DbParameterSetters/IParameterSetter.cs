using System;
using System.Data.Common;
using VODB.Core.Infrastructure;

namespace VODB.Core.Execution.DbParameterSetters
{
    public interface IParameterSetter
    {
        Boolean CanHandle(Type type);

        void SetValue(DbParameter param, Field field, Object value);
    }
}