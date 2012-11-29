using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using VODB.Extensions;

namespace VODB.ExpressionParser
{
    /// <summary>
    /// Formats the condition...
    /// </summary>
    public interface IWhereExpressionFormatter
    {

        /// <summary>
        /// Gives a piece of the expression using the fieldname and the parametername.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <returns></returns>
        String Format(String fieldName, String parameterName);

        /// <summary>
        /// Determines whether this instance can format the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        Boolean CanFormat(ExpressionType type);

    }
}
