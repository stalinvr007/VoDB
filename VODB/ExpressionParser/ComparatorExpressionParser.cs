using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace VODB.ExpressionParser
{
    class ComparatorExpressionParser<TModel> : IExpressionParser<Func<TModel, Boolean>>
        where TModel : Entity
    {
        /// <summary>
        /// Parses the specified expression.
        /// </summary>
        /// <typeparam name="TDelegate">The type of the delegate.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public String Parse(Expression<Func<TModel, Boolean>> expression)
        {
            BinaryExpression body = (BinaryExpression)expression.Body;

            MemberExpression right = (MemberExpression)body.Right;
            var value = Expression.Lambda(right).Compile().DynamicInvoke();
            var field = ((MemberExpression)body.Left).Member.Name;
            
            return String.Format("{0} = '{1}'", field, value);
        }
        
    }
}
