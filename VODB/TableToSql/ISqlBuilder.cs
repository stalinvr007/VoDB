using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VODB.Infrastructure;

namespace VODB.TableToSql
{
    /// <summary>
    /// Builds a SQL Command.
    /// </summary>
    public interface ISqlBuilder
    {
        /// <summary>
        /// Builds the SQL for the specified table.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        String Build(ITable table);
    }
}