using System;
using System.Linq.Expressions;
using VODB.Core;
using VODB.Core.Infrastructure;

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

        #region IExpressionParser<Func<TEntity,TField>> Members

        /// <summary>
        /// Parses the specified expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public String Parse(Expression<Func<TEntity, TField>> expression)
        {
            var memberExp = expression.Body as MemberExpression;
            if (memberExp == null)
            {
                memberExp = ((UnaryExpression)expression.Body).Operand as MemberExpression;
            }

            string field = memberExp.Member.Name;

            Field = Engine.GetTable<TEntity>().FindField(field);

            return Field.FieldName;
        }

        #endregion
    }
}