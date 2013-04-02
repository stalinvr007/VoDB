using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VODB.Core.Execution.Executers.DbResults;
using VODB.ExpressionsToSql;

namespace VODB.DbLayer
{
    class VodbCommand : IVodbCommand
    {
        private readonly DbCommand _Command;

        private DbParameter InternalCreateParameter(String name, Object value)
        {
            var parameter = _Command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value;
            return parameter;
        }

        public void AddParameter(IQueryParameter parameter)
        {
            _Command.Parameters.Add(
                InternalCreateParameter(parameter.Name, parameter.Value)
            );
        }

        public void CreateParameter(String name, Object value)
        {
            var parameter = InternalCreateParameter(name, value);

            if (parameter.Value == null)
            {
                parameter.Value = DBNull.Value;
            }

            _Command.Parameters.Add(parameter);
        }

        private static bool InvalidDateTime(IQueryParameter parameter)
        {
            return parameter.type == typeof(DateTime) && ((DateTime)parameter.Value).Year < 1753;
        }
        public void RefreshParametersValues(IEnumerable<IQueryParameter> parameters)
        {
            int i = -1;
            foreach (var parameter in parameters)
            {
                if (InvalidDateTime(parameter))
                {
                    _Command.Parameters[++i].Value = DBNull.Value;
                }
                else
                {
                    _Command.Parameters[++i].Value = parameter.Value ?? DBNull.Value;
                }
            }
        }

        public void CreateParameters(IEnumerable<string> names)
        {
            foreach (var name in names)
            {
                _Command.Parameters.Add(InternalCreateParameter(name, DBNull.Value));
            }
        }

        public void SetCommandText(String sql)
        {
            _Command.CommandText = sql;
        }

        public void SetTransaction(DbTransaction transaction)
        {
            _Command.Transaction = transaction;
        }

        public void SetConnection(DbConnection connection)
        {
            _Command.Connection = connection;
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
