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

        private readonly LambdaExpression _Expression;
        private readonly IExpressionBreaker _Breaker;
        private readonly ICollection<IQueryParameter> _Parameters = new List<IQueryParameter>();
        private readonly IQueryCondition _Follows;

        private QueryCondition(IEntityTranslator translator, LambdaExpression expression)
        {
            _Expression = expression;
            _Breaker = new ExpressionBreaker(translator);
        }

        public QueryCondition(IEntityTranslator translator, Expression<Func<TEntity, Boolean>> expression)
            : this(translator, (LambdaExpression)expression)
        {
            _Follows = CreateFollowCondition(_Expression);
        }

        public QueryCondition(IEntityTranslator translator, Expression<Func<TEntity, Object>> expression, IQueryCondition follows)
            : this(translator, (LambdaExpression)expression)
        {
            _Follows = follows;
        }

        private static IQueryCondition CreateFollowCondition(LambdaExpression expression)
        {
            switch (expression.Body.NodeType)
            {
                default: throw new InvalidOperationException("Unable to decode this node type. " + expression.Body.NodeType.ToString());
            }
        }

        private void AppendParameter(int level)
        {
            var value = _Expression.GetRightValue();
            if (value != null)
            {
                _Parameters.Add(new QueryParameter
                {
                    Name = "@p" + level,
                    Value = value
                });
            }
        }
        public String Compile()
        {
            _Parameters.Clear();
            var parts = _Breaker.BreakExpression(_Expression).ToList();

            var sb = new StringBuilder();

            SafePrint(sb, parts[0].Field.Name);

            for (int i = 1; i < parts.Count; ++i)
            {
                var current = parts[i].Field;
                var next = parts[i - 1].Field;

                sb.Append(" in (Select [")
                    .Append(next.BindOrName)
                    .Append("] From [")
                    .Append(current.Table.Name)
                    .Append("] Where ");

                SafePrint(sb, current.Name);
            }

            sb.Append(_Follows.Compile());

            AppendParameter(0);

            foreach (var parameter in _Follows.Parameters)
            {
                _Parameters.Add(parameter);
            }

            for (int i = 0; i < parts.Count - 1; i++)
            {
                sb.Append(")");
            }

            return sb.ToString();

        }

        private StringBuilder SafePrint(StringBuilder sb, String name)
        {
            return sb.Append(LEFT_BRACKET).Append(name).Append(RIGHT_BRACKET);
        }

        public IEnumerable<IQueryParameter> Parameters
        {
            get { return _Parameters; }
        }

    }

}
