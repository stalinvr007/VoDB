using System.Collections.Generic;

namespace VODB.DbLayer.DbExecuters
{
    /// <summary>
    /// Represents a query to be executed against the database.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    interface IQueryExecuter<out TResult>
    {

        /// <summary>
        /// Executes this query.
        /// </summary>
        /// <returns></returns>
        IEnumerable<TResult> Execute();

    }
}
