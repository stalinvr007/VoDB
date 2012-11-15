using System.Collections.Generic;

namespace VODB.DbLayer.DbExecuters
{
    /// <summary>
    /// Represents a query to be executed against the database.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    interface IQueryExecuter<TResult> : IEnumerable<TResult>
    {

        /// <summary>
        /// Creates the DbQueryResult to be executed later.
        /// </summary>
        /// <returns></returns>
        IDbQueryResult<TResult> Execute();


        /// <summary>
        /// Executes this query.
        /// </summary>
        /// <returns></returns>
        IEnumerable<TResult> InternalExecute();

    }
}
