﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VODB.ExpressionParser.TSqlBuilding
{
    public interface ITSqlBuilder
    {

        /// <summary>
        /// Determines whether this instance can build the Sql using the specified parser.
        /// </summary>
        /// <param name="parser">The parser.</param>
        /// <returns></returns>
        Boolean CanBuild(IExpressionBodyParser parser);

        /// <summary>
        /// Gets the parameters used on the Sql Statement.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        IEnumerable<KeyValuePair<Key, Object>> Parameters { get; }

        /// <summary>
        /// Builds Sql.
        /// </summary>
        /// <returns></returns>
        String Build();

    }
}
