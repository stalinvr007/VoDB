using System;
using System.Collections.Generic;

namespace VODB.ExpressionParser.TSqlBuilding
{
    public interface ITSqlBuilder
    {
        /// <summary>
        /// Gets the parameters used on the Sql Statement.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        IEnumerable<KeyValuePair<Key, Object>> Parameters { get; }

        /// <summary>
        /// Determines whether this instance can build the Sql using the specified parser.
        /// </summary>
        /// <param name="parser">The parser.</param>
        /// <returns></returns>
        Boolean CanBuild(IExpressionBodyParser parser);

        /// <summary>
        /// Clears the parameters.
        /// </summary>
        void ClearParameters();

        /// <summary>
        /// Builds Sql.
        /// </summary>
        /// <param name="paramCount">The param count.</param>
        /// <returns></returns>
        String Build(int paramCount);
    }
}