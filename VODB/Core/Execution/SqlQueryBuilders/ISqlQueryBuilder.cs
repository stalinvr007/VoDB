using System;
using System.Collections.Generic;
using VODB.Core.Infrastructure;

namespace VODB.Core.Execution.SqlPartialBuilders
{
    internal interface ISqlQueryBuilder
    {
        /// <summary>
        /// Gets the query.
        /// </summary>
        /// <value>
        /// The query.
        /// </value>
        String Query { get; }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        IEnumerable<Parameter> Parameters { get; }

        /// <summary>
        /// Adds the condition.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        ISqlQueryBuilder AddCondition(Field field, Object entity);
    }
}