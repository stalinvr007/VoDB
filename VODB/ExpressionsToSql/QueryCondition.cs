using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VODB.ExpressionsToSql
{
    class QueryCondition : IQueryConditionComposite
    {

        private readonly IList<IQueryCondition> queries = new List<IQueryCondition>();

        public void Add(IQueryCondition query)
        {
            queries.Add(query);
        }

        public String Compile(ref int level)
        {
            var sb = new StringBuilder();

            foreach (var query in queries)
            {
                 sb.Append(query.Compile(ref level));
            }

            return sb.ToString();
        }
        
        public IEnumerable<IQueryParameter> Parameters
        {
            get { return queries.SelectMany(q => q.Parameters); }
        }
    }
}
