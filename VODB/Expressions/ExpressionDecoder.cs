using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using VODB.Core;
using VODB.EntityTranslation;
using VODB.Exceptions;

namespace VODB.Expressions
{

    class ExpressionDecoder<TEntity, TReturnValue> : IExpressionDecoder
    {
        private readonly LambdaExpression _Expression;
        private readonly IEntityTranslator _Translator;

        public ExpressionDecoder(IEntityTranslator translator, Expression<Func<TEntity, TReturnValue>> expression)
        {
            _Translator = translator;
            _Expression = expression;
        }

        public Boolean HasParameters
        {
            get { return _Expression.Body is BinaryExpression; }
        }

        public ExpressionType NodeType
        {
            get { return _Expression.Body.NodeType; }
        }

        public IEnumerable<ExpressionPart> DecodeLeft()
        {
            var current = GetFirstMember(_Expression);

            while (current != null)
            {
                var part = new ExpressionPart
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

        private MemberExpression GetFirstMember(LambdaExpression expression)
        {
            if (expression.Body is BinaryExpression)
                return ((BinaryExpression)expression.Body).Left as MemberExpression;

            if (expression.Body is MemberExpression)
                return expression.Body as MemberExpression;

            if (expression.Body is UnaryExpression && expression.Body.NodeType == ExpressionType.Convert)
                return ((UnaryExpression)expression.Body).Operand as MemberExpression;

            throw new UnableToGetTheFirstMember(expression.ToString());
        }

        public IEnumerable<Object> DecodeRight()
        {
            var exp = _Expression.Body as BinaryExpression;

            if (exp == null)
            {
                return new Object[] { };
            }

            var constantExpression = exp.Right as ConstantExpression;
            if (constantExpression != null)
            {
                return new[] { constantExpression.Value };
            }

            return new[] { Expression.Lambda(exp.Right).Compile().DynamicInvoke() };
        }

    }

}
