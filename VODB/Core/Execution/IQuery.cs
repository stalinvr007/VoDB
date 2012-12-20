using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VODB.Core.Execution
{
    /// <summary>
    /// Represents a Query
    /// </summary>
    public interface IQuery
    {

        /// <summary>
        /// Gets the query.
        /// </summary>
        /// <value>
        /// The query.
        /// </value>
        String Query { get; }

    }
}
