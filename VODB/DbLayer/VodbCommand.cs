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
    class VodbCommand : IVodbCommand
    {
        private readonly DbCommand _Command;

        public void CreateParameter(String name, Object value)
        {
            var parameter = _Command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value;

            if (parameter.Value == null)
            {
                parameter.Value = DBNull.Value;
            }

            _Command.Parameters.Add(parameter);
        }

        public void SetCommandText(String sql)
        {
            _Command.CommandText = sql;
        }

        public void SetTransaction(DbTransaction transaction)
        {
            _Command.Transaction = transaction;
        }

        public VodbCommand(DbCommand command)
        {
            _Command = command;
        }

        public int ExecuteNonQuery()
        {
            return _Command.ExecuteNonQuery();
        }

        public IDataReader ExecuteReader()
        {
            return _Command.ExecuteReader();
        }

        public object ExecuteScalar()
        {
            return _Command.ExecuteScalar();
        }

    }
}
