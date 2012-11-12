using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VODB.DbLayer
{
    /// <summary>
    /// Represents a query to be executed against the database.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    interface IQuery<TResult>
    {

        /// <summary>
        /// Executes this query.
        /// </summary>
        /// <returns></returns>
        IEnumerable<TResult> Execute();

    }
}
