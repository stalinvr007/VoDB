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

    class ExpressionPart
    {
        public String PropertyName { get; set; }
        public Field Field { get; set; }
        public Type EntityType { get; set; }
        public Table EntityTable { get; set; }
        public Object Value { get; set; }
    }

    class ExpressionDecoder<TEntity>
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
                yield return new ExpressionPart 
                { 
                    PropertyName = current.Member.Name,
                    EntityType = current.Member.DeclaringType,
                    EntityTable = Engine.GetTable(current.Member.DeclaringType)
                };

                current = current.Expression as MemberExpression;
            }

        }

    }

}
