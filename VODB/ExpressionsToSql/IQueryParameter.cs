using System;
using VODB.Infrastructure;

namespace VODB.ExpressionsToSql
{
    public interface IQueryParameter
    {
        String Name { get; }
        Object Value { get; set; }
        Type type { get; }
        IField Field { get; set; }
    }
}
