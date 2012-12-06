using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace VODB.ExpressionParser
{
    public class FieldGetterExpressionParser<TEntity, TField> : IExpressionParser<Func<TEntity, TField>>
         where TEntity : Entity, new()
    {
        /// <summary>
        /// Parses the specified expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public String Parse(Expression<Func<TEntity, TField>> expression)
        {

            var entity = new TEntity();
            string field = ((MemberExpression)expression.Body).Member.Name;
            var tableField = entity.Table.Fields.First(f => f.PropertyName.Equals(field, StringComparison.InvariantCultureIgnoreCase));

            return String.Format(" Order by {0}", tableField.FieldName);
        }
    }
}
