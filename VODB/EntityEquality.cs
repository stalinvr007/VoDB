using System.Collections.Generic;
using System.Linq;

namespace VODB
{
    public sealed class EntityEquality<TEntity> : IEqualityComparer<TEntity>
            where TEntity : Entity
    {

        public bool Equals(TEntity x, TEntity y)
        {
            var xKeys = x.Table.KeyFields;
            var yKeys = y.Table.KeyFields;

            var firstOrDefault =
                xKeys.Zip(yKeys, (xKey, yKey) => new { xVal = xKey.GetValue(x), yVal = yKey.GetValue(y) })
                     .FirstOrDefault(a =>
                                a.xVal == null && a.yVal != null ||
                                a.xVal != null && !a.xVal.Equals(a.yVal));

            return firstOrDefault == null;

        }

        public int GetHashCode(TEntity obj)
        {
            return obj.Table.KeyFields.Select(f => f.GetValue(obj))
                .Sum(val => val == null ? 0 : val.GetHashCode());
        }
    }
}
