using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace VODB.ExpressionParser
{

    /// <summary>
    /// Parses an Expression to something else.
    /// </summary>
    /// <typeparam name="TDelegate">The type of the delegate.</typeparam>
    public interface IExpressionParser<TDelegate>
    {

        /// <summary>
        /// Parses the specified expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        String Parse(Expression<TDelegate> expression);

    }

}
