using System;

namespace VODB.ExpressionsToSql
{
    public interface IQueryParameter
    {
        String Name { get; }
        Object Value { get; }
        Type type { get; }
    }
}
