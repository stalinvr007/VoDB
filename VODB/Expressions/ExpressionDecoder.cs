﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using VODB.Core;
using VODB.Exceptions;

namespace VODB.Expressions
{

    class ExpressionDecoder<TEntity, TReturnValue> : IExpressionDecoder
    {
        private readonly LambdaExpression _Expression;

        public ExpressionDecoder(Expression<Func<TEntity, TReturnValue>> expression)
        {
            _Expression = expression;
        }

        public Boolean HasParameters
        {
            get { return _Expression.Body is BinaryExpression; }
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

        public IEnumerable<Object> DecodeRight()
        {
            var exp = _Expression.Body as BinaryExpression;

            if (exp == null)
            {
                throw new InvalidProgramException("Can't get the right part of expression.");
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
