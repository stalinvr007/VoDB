using System;
using System.Collections.Generic;
using VODB.Expressions;

namespace VODB.ExpressionsToSql
{
    public interface IQueryCondition
    {

        /// <summary>
        /// Compiles the query.
        /// </summary>
        /// <returns></returns>
        String Compile();

        /// <summary>
        /// Gets the parameter values.
        /// </summary>
        /// <value>
        /// The parameter values.
        /// </value>
        IEnumerable<IQueryParameter> Parameters { get; }

    }
}
