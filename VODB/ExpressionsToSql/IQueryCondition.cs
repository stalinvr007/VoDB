﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VODB.ExpressionsToSql
{
    public interface IQueryCondition
    {
        /// <summary>
        /// Compiles the query given a spefic depth level.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <returns></returns>
        String Compile(int level);

        /// <summary>
        /// Gets the parameter values.
        /// </summary>
        /// <value>
        /// The parameter values.
        /// </value>
        IEnumerable<IQueryParameter> Parameters { get; }
    }
}