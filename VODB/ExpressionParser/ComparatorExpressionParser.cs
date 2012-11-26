using System;
using System.Linq.Expressions;
using VODB.VirtualDataBase;
using System.Linq;

namespace VODB.ExpressionParser
{
    class ComparatorExpressionParser<TModel> : IExpressionParser<Func<TModel, Boolean>>
        where TModel : Entity, new()
    {

        private static Table Table = new TModel().Table;

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

            var tableField = Table.Fields.First(f => f.PropertyName.Equals(field, StringComparison.InvariantCultureIgnoreCase));

            return String.Format("{0} = '{1}'", tableField.FieldName, value);
        }
        


    }
}
