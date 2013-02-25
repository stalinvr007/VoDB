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
                        
            return Build(parts, 0, level);
        }


        private static String Build(IList<ExpressionPart> parts, int index, int level)
        {

            if (index == parts.Count - 1)
            {
                return String.Format("{0} = @p{1}",
                    parts[index].Field.FieldName,
                    level);
            }

            var current = parts[index].Field;
            var next = parts[index + 1].Field;

            return String.Format("{0} in (Select {1} From {2} Where {3})",
                current.FieldName,
                next.BindedTo ?? next.FieldName,
                current.Table.TableName,
                Build(parts, index + 1, level));
        }


    }

}
