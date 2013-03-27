using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VODB.Core.Execution.Executers.DbResults;
using VODB.DbLayer;

namespace VODB.DbLayer
{

    /// <summary>
    /// The representation of a logical connection.
    /// </summary>
    public interface IVodbConnection : IVodbCommandFactory, IDisposable
    {

        /// <summary>
        /// Begins the transaction.
        /// </summary>
        /// <returns></returns>
        IVodbTransaction BeginTransaction();

        /// <summary>
        /// Asks the manager to open the connection.
        /// </summary>
        void Open();
        
        /// <summary>
        /// Asks the manager to close the connection.
        /// </summary>
        void Close();

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>The number of records afected by the command.</returns>
        int ExecuteNonQuery(IVodbCommand command);

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>A DataReader with the result</returns>
        IDataReader ExecuteReader(IVodbCommand command);

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>A simgle value.</returns>
        object ExecuteScalar(IVodbCommand command);
    }
}
