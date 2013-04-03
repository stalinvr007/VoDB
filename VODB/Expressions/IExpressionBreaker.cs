using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace VODB.Expressions
{
    /// <summary>
    /// Breaks the expression into pieces.
    /// </summary>
    public interface IExpressionBreaker
    {
        /// <summary>
        /// Breaks the expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        IEnumerable<IExpressionPiece> BreakExpression(LambdaExpression expression);
    }
}
