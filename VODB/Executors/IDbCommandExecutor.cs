using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using ConcurrentReader;

namespace VODB.Executors
{
    /// <summary>
    /// Executes a command
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public interface IDbCommandExecutor<TResult>
    {

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        TResult ExecuteCommand(DbCommand command);
    }
}