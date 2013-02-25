using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VODB.Core.Infrastructure;
using VODB.ExpressionParser;

namespace VODB.ExpressionsToSql
{

    static class QueryExtensions
    {

        public static String ListToString(this IEnumerable<String> lines)
        {
            var sb = new StringBuilder();

            foreach (var item in lines)
            {
                sb.Append(item);
            }

            return sb.ToString();
        }

        public static void Append(this LinkedList<String> lines, String mask, params Object[] values)
        {
            lines.AddFirst(String.Format(mask, values));
        }

    }

    class Query<TEntity>
    {

        private readonly IExpressionDecoder _Expression;

        public Query(Expression<Func<TEntity, Boolean>> expression)
        {
            _Expression = new ExpressionDecoder<TEntity>(expression);
        }

        public String Compile(int level)
        {
            var parts = _Expression.DecodeLeft().ToList();
            //parts.Reduce();

            var sb = new StringBuilder();
            Build(sb, parts, 0, level);
            return sb.ToString();
        }


        private static void Build(StringBuilder sb, IList<ExpressionPart> parts, int index, int level)
        {

            if (index == parts.Count - 1)
            {
                sb.Append(parts[index].Field.FieldName).Append(" = @p").Append(level);
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


    }

}
