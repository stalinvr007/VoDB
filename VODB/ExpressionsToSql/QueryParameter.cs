using System;

namespace VODB.ExpressionsToSql
{
    class QueryParameter : IQueryParameter
    {
        public String Name { get; set; }
        public Object Value { get; set; }
    }
}
