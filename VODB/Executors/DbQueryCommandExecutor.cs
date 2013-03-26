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
    class DbQueryCommandExecutor : IDbCommandExecutor<IDataReader>
    {
        public IDataReader ExecuteCommand(IVodbCommand command)
        {
            return command.ExecuteReader().AsParallel();
        }
    }
}
