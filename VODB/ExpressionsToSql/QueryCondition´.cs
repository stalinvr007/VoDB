using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using VODB.EntityTranslation;
using VODB.Expressions;

namespace VODB.ExpressionsToSql
{

    class QueryCondition<TEntity> : IQueryCondition
    {
        private const string RIGHT_BRACKET = "]";
        private const string LEFT_BRACKET = "[";

        private readonly IExpressionDecoder _Expression;
        private readonly ICollection<IQueryParameter> _Parameters = new List<IQueryParameter>();
        private readonly IQueryCondition _Follows;

        public QueryCondition(IEntityTranslator translator, Expression<Func<TEntity, Boolean>> expression)
        {
            _Expression = new ExpressionDecoder<TEntity, Boolean>(translator, expression);
            _Follows = CreateFollowCondition(_Expression);
        }

        public QueryCondition(IEntityTranslator translator, Expression<Func<TEntity, Object>> expression, IQueryCondition follows)
        {
            _Follows = follows;
            _Expression = new ExpressionDecoder<TEntity, Object>(translator, expression);
        }

        private static IQueryCondition CreateFollowCondition(IExpressionDecoder decoder)
        {
            switch (decoder.NodeType)
            {
                case ExpressionType.Equal: return new ParameterCondition(" = @p");
                case ExpressionType.GreaterThan: return new ParameterCondition(" > @p");
                case ExpressionType.GreaterThanOrEqual: return new ParameterCondition(" >= @p");
                case ExpressionType.LessThan: return new ParameterCondition(" < @p");
                case ExpressionType.LessThanOrEqual: return new ParameterCondition(" <= @p");
                default: throw new InvalidOperationException("Unable to decode this node type. " + decoder.NodeType.ToString());
            }
        }

        public String Compile(ref int level)
        {
            _Parameters.Clear();
            var parts = _Expression.DecodeLeft().ToList();

            var sb = new StringBuilder();
            Build(sb, parts, 0, ref level);
            return sb.ToString();
        }

        private void Build(StringBuilder sb, IList<ExpressionPart> parts, int index, ref int level)
        {

            if (index == parts.Count - 1)
            {
                Debug.Assert(parts[index] != null);
                Debug.Assert(parts[index].Field != null);

                // Finalize the Condition
                sb.Append(LEFT_BRACKET).Append(parts[index].Field.Name).Append(RIGHT_BRACKET)
                  .Append(_Follows.Compile(ref level));

                // Add the values as a parameter to the parameters collection.
                foreach (var value in _Expression.DecodeRight())
                {
                    _Parameters.Add(new QueryParameter
                    {
                        Name = "@p" + level,
                        Value = value
                    });
                }

                foreach (var parameter in _Follows.Parameters)
                {
                    _Parameters.Add(parameter);
                }

                return;
            }

            var current = parts[index].Field;
            var next = parts[index + 1].Field;

            sb.Append(LEFT_BRACKET).Append(current.Name).Append(RIGHT_BRACKET)
                .Append(" in (Select [").Append(next.BindOrName)
                .Append("] From [")
                .Append(current.Table.Name)
                .Append("] Where ");

            Build(sb, parts, index + 1, ref level);
            sb.Append(")");
        }

        public IEnumerable<IQueryParameter> Parameters
        {
            get { return _Parameters; }
        }


    }

}
