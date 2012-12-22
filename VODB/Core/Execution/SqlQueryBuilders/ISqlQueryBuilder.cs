using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VODB.Core.Infrastructure;
using VODB.ExpressionParser;

namespace VODB.Core.Execution.SqlPartialBuilders
{
    interface ISqlQueryBuilder
    {

        /// <summary>
        /// Adds the condition.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        ISqlQueryBuilder AddCondition(Field field, Object entity);

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
    }
}
