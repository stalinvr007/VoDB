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
        void CreateParameter(String name, Object value);
        void SetCommandText(String sql);
        void SetTransaction(DbTransaction transaction);

        int ExecuteNonQuery();
        IDataReader ExecuteReader();
        object ExecuteScalar();
    }
}
