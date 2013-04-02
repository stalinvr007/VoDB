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
    /// <summary>
    /// This is a wrapper for DbConnection object.
    /// </summary>
    public interface IVodbCommand
    {
        /// <summary>
        /// Creates the parameter with the given name and value.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        void AddParameter(IQueryParameter parameter);

        /// <summary>
        /// Refreshes the parameters values.
        /// 
        /// Having in account that the order of the values must be the same
        /// as the created parameters.
        /// </summary>
        /// <param name="values">The values.</param>
        void RefreshParametersValues(IEnumerable<IQueryParameter> values);
        /// <summary>
        /// Creates the parameters with the given names and values equal to DBNull.value.
        /// </summary>
        /// <param name="names">The names.</param>
        void CreateParameters(IEnumerable<String> names);

        void SetCommandText(String sql);
        void SetTransaction(DbTransaction transaction);
        void SetConnection(DbConnection connection);

        int ExecuteNonQuery();
        IDataReader ExecuteReader();
        object ExecuteScalar();
    }
}
