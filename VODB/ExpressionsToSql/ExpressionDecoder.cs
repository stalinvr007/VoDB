using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VODB.Core;
using VODB.Core.Infrastructure;

namespace VODB.ExpressionsToSql
{

    class ExpressionDecoder<TEntity> : IExpressionDecoder
    {
        private Expression<Func<TEntity, Boolean>> _Expression;

        public ExpressionDecoder(Expression<Func<TEntity, Boolean>> expression)
        {
            _Expression = expression;
        }

        public IEnumerable<ExpressionPart> DecodeLeft()
        {
            var exp = _Expression.Body as BinaryExpression;

            var current = exp.Left as MemberExpression;

            while (current != null)
            {
                var part = new ExpressionPart 
                { 
                    PropertyName = current.Member.Name,
                    EntityType = current.Member.DeclaringType
                };

                part.EntityTable = Engine.GetTable(part.EntityType);
                part.Field = part.EntityTable.FindField(part.PropertyName);

                yield return part;

                current = current.Expression as MemberExpression;
            }
        }

        

        public Object DecodeRight()
        {
            var exp = _Expression.Body as BinaryExpression;

            if (exp.Right is ConstantExpression)
            {
                return ((ConstantExpression)exp.Right).Value;
            }
            else
            {
                return Expression.Lambda(exp.Right).Compile().DynamicInvoke();
            }
        }

    }

}
