using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using VODB.Expressions;

namespace VODB.ExpressionsToSql
{

    class QueryCondition<TEntity> : IQueryCondition
    {

        private readonly IExpressionDecoder _Expression;
        private readonly ICollection<IQueryParameter> _Parameters = new List<IQueryParameter>();
        private readonly IQueryCondition _Follows;

        public QueryCondition(Expression<Func<TEntity, Boolean>> expression)
        {
            _Expression = new ExpressionDecoder<TEntity, Boolean>(expression);
            _Follows = CreateFollowCondition(_Expression);
        }

        public QueryCondition(Expression<Func<TEntity, Object>> expression, IQueryCondition follows)
        {
            _Follows = follows;
            _Expression = new ExpressionDecoder<TEntity, Object>(expression);
        }

        private static IQueryCondition CreateFollowCondition(IExpressionDecoder decoder)
        {
            switch (decoder.NodeType)
            {
                case ExpressionType.Equal: return new ConstantCondition(" = @p");
                case ExpressionType.GreaterThan: return new ConstantCondition(" > @p");
                default: throw new InvalidOperationException("Unable to decode this node type. " + decoder.NodeType.ToString());
            }
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
                sb.Append(parts[index].Field.FieldName)
                  .Append(_Follows.Compile(level));

                // Add the values as a parameter to the parameters collection.
                foreach (var value in _Expression.DecodeRight())
                {
                    _Parameters.Add(new QueryParameter
                    {
                        Name = "@p" + level,
                        Value = value
                    });    
                }
                
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
