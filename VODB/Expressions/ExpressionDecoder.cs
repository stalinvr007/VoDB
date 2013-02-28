using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using VODB.Core;
using VODB.Exceptions;

namespace VODB.Expressions
{

    class ExpressionDecoder<TEntity, TReturnValue> : IExpressionDecoder
    {
        private LambdaExpression _Expression;

        public ExpressionDecoder(Expression<Func<TEntity, TReturnValue>> expression)
        {
            _Expression = expression;
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

                part.NodeType = _Expression.Body.NodeType;
                part.EntityTable = Engine.GetTable(part.EntityType);
                part.Field = part.EntityTable.FindField(part.PropertyName);

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

            throw new UnableToGetTheFirstMember(expression.ToString());
        }

        public Object DecodeRight()
        {
            var exp = _Expression.Body as BinaryExpression;

            var constantExpression = exp.Right as ConstantExpression;
            if (constantExpression != null)
            {
                return constantExpression.Value;
            }
            
            return Expression.Lambda(exp.Right).Compile().DynamicInvoke();
        }

    }

}
