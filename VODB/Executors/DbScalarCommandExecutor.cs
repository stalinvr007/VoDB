using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using ConcurrentReader;
using VODB.DbLayer;

namespace VODB.Executors
{
    public class DbScalarCommandExecutor : IDbCommandExecutor<Object>
    {
        public Object ExecuteCommand(IVodbCommand command)
        {
            return command.ExecuteScalar();
        }
    }
}
