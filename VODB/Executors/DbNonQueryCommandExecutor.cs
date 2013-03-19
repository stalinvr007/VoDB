using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using ConcurrentReader;

namespace VODB.Executors
{
    public class DbNonQueryCommandExecutor : IDbCommandExecutor<int>
    {
        public int ExecuteCommand(DbCommand command)
        {
            return command.ExecuteNonQuery();
        }
    }
}
