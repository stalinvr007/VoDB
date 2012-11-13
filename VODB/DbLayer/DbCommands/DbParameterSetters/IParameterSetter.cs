using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using VODB.VirtualDataBase;

namespace VODB.DbLayer.DbCommands.DbParameterSetters
{
    public interface IParameterSetter
    {

        Boolean CanHandle(Type type);

        void SetValue(DbParameter param, Field field, Object entity);

    }
}
