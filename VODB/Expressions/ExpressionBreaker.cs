using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using VODB.EntityTranslation;
using VODB.Exceptions;

namespace VODB.Expressions
{
    class ExpressionBreaker : IExpressionBreaker
    {
        private readonly IEntityTranslator _Translator;

        public ExpressionBreaker(IEntityTranslator translator)
        {
            _Translator = translator;
        }

        /// <summary>
        /// Breaks the expression.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TReturnValue">The type of the return value.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public IEnumerable<IExpressionPiece> BreakExpression(LambdaExpression expression)
        {
            var current = GetFirstMember(expression);

            while (current != null)
            {
                var part = new ExpressionPiece
                {
                    PropertyName = current.Member.Name,
                    EntityType = current.Member.DeclaringType
                };

                // Gets the table for this entity type.
                part.EntityTable = _Translator.Translate(part.EntityType);

                // Gets the field used in this expression part.
                part.Field = part.EntityTable
                    .Fields
                    .FirstOrDefault(f => f.Info.Name == part.PropertyName);

                Debug.Assert(part.Field != null, "The property [" + part.PropertyName + "] used in the expression doesn't belong to the Entity table representation.");

                yield return part;

                current = current.Expression as MemberExpression;
            }
        }

        private static MemberExpression GetFirstMember(LambdaExpression expression)
        {
            if (expression.Body is BinaryExpression)
                return ((BinaryExpression)expression.Body).Left as MemberExpression;

            if (expression.Body is MemberExpression)
                return expression.Body as MemberExpression;

            if (expression.Body is UnaryExpression && expression.Body.NodeType == ExpressionType.Convert)
                return ((UnaryExpression)expression.Body).Operand as MemberExpression;

            throw new UnableToGetTheFirstMember(expression.ToString());
        }

        public override string ToString()
        {
            return "V2.0";
        }
    }
}
