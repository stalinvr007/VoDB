using System;

namespace VODB.ExpressionsToSql
{
    public interface IQueryParameter
    {
        String Name { get; }
        Object Value { get; set; }
        Type type { get; }
    }
}
