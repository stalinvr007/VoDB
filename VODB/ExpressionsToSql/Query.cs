using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VODB.Core.Infrastructure;
using VODB.ExpressionParser;

namespace VODB.ExpressionsToSql
{
    class Query : IQuery, IQueryComposite
    {

        private readonly IList<IQuery> queries = new List<IQuery>();

        public void Add(IQuery query)
        {
            queries.Add(query);
        }

        public String Compile(int level)
        {
            var sb = new StringBuilder();

            Parallel.ForEach(queries, q =>
            {
                SafeAppendLine(sb, q.Compile(Interlocked.Increment(ref level)));
            });

            return sb.ToString();
        }

        private void SafeAppendLine(StringBuilder sb, String text)
        {
            lock (sb)
            {
                sb.AppendLine(text);
            }
        }

    }
}
