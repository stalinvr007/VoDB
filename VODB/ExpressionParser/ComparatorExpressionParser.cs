using System;
using System.Linq.Expressions;

namespace VODB.ExpressionParser
{
    class ComparatorExpressionParser<TModel> : IExpressionParser<Func<TModel, Boolean>>
        where TModel : Entity
    {
        /// <summary>
        /// Parses the specified expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public String Parse(Expression<Func<TModel, Boolean>> expression)
        {
            var body = (BinaryExpression)expression.Body;

            var right = (MemberExpression)body.Right;
            var value = Expression.Lambda(right).Compile().DynamicInvoke();
            var field = ((MemberExpression)body.Left).Member.Name;
            
            return String.Format("{0} = '{1}'", field, value);
        }
        
    }
}
