using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VODB.Core.Infrastructure;
using VODB.ExpressionParser;
using VODB.Expressions;

namespace VODB.ExpressionsToSql
{
    class QueryLeft<TEntity, TFieldValue> : IQueryCondition
    {
        private readonly IExpressionDecoder _Expression;

        public QueryLeft(Expression<Func<TEntity, TFieldValue>> expression)
        {

        }

        public string Compile(int level)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IQueryParameter> Parameters
        {
            get { throw new NotImplementedException(); }
        }
    }

    class QueryCondition<TEntity> : IQueryCondition
    {

        private readonly IExpressionDecoder _Expression;
        private ICollection<IQueryParameter> _Parameters = new List<IQueryParameter>();

        public QueryCondition(Expression<Func<TEntity, Boolean>> expression)
        {
            _Expression = new ExpressionDecoder<TEntity, Boolean>(expression);
        }
        
        public String Compile(int level)
        {
            var parts = _Expression.DecodeLeft().ToList();

            var sb = new StringBuilder();
            Build(sb, parts, 0, level);
            return sb.ToString();
        }

        private void Build(StringBuilder sb, IList<ExpressionPart> parts, int index, int level)
        {

            if (index == parts.Count - 1)
            {
                // Finalize the Condition
                sb.Append(parts[index].Field.FieldName).Append(" = @p").Append(level);

                // Add the parameter to the parameters collection.
                _Parameters.Add(new QueryParameter
                {
                    Name = "@p" + level,
                    Value = _Expression.DecodeRight()
                });

                return;
            }

            var current = parts[index].Field;
            var next = parts[index + 1].Field;

            sb.Append(current.FieldName)
                .Append(" in (Select ").Append(next.BindedTo ?? next.FieldName)
                .Append(" From ")
                .Append(current.Table.TableName)
                .Append(" Where ");

            Build(sb, parts, index + 1, level);
            sb.Append(")");
        }

        public IEnumerable<IQueryParameter> Parameters
        {
            get { return _Parameters; }
        }


    }

}
