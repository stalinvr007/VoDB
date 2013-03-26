using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VODB.Core.Execution.Executers.DbResults;

namespace VODB.DbLayer
{
    public interface IVodbCommand
    {
        DbParameterCollection Parameters { get; }
        DbParameter CreateParameter();
        void SetCommandText(String sql);
        int ExecuteNonQuery();
        IDataReader ExecuteReader();
        void SetTransaction(DbTransaction transaction);
        object ExecuteScalar();
    }
}
