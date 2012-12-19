using System;
using System.Linq;
using System.Linq.Expressions;
using VODB.Core.Infrastructure;
using VODB.Extensions;


namespace VODB.ExpressionParser
{
    public class FieldGetterExpressionParser<TEntity, TField> : IExpressionParser<Func<TEntity, TField>>
         where TEntity : new()
    {

        /// <summary>
        /// Gets the field that was found by the parser.
        /// </summary>
        /// <value>
        /// The field.
        /// </value>
        public Field Field { get; private set; }

        /// <summary>
        /// Parses the specified expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public String Parse(Expression<Func<TEntity, TField>> expression)
        {

            var entity = new TEntity() as Entity;
            string field = ((MemberExpression)expression.Body).Member.Name;

            Field = entity.FindField(field);

            return Field.FieldName;
        }
    }
}
