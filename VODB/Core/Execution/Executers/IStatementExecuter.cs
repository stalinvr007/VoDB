using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using VODB.Core.Execution.Statements;
using VODB.Core.Infrastructure;

namespace VODB.Core.Execution.Executers
{
    /// <summary>
    /// Represents a StatementExecuter.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    interface IStatementExecuter<TResult>
    {

        /// <summary>
        /// Executes a command using the specified entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="session">The session.</param>
        /// <returns></returns>
        TResult Execute<TEntity>(TEntity entity, IInternalSession session);

    }
}
