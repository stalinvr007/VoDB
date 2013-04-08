using System;
using VODB.Infrastructure;

namespace VODB.ExpressionsToSql
{
    class QueryParameter : IQueryParameter
    {
        public String Name { get; set; }
        public Object Value { get; set; }
        public Type type { get; set; }
        public IField Field { get; set; }
    }
}
