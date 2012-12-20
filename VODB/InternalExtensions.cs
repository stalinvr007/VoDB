using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VODB.Core;
using VODB.Core.Infrastructure;

namespace VODB
{
    internal static class InternalExtensions
    {

        public static Table GetTable<TEntity>(this TEntity entity)
        {
            return Engine.GetTable<TEntity>();
        }

        public static Boolean IsEntity(this Type entityType)
        {
            return Engine.IsMapped(entityType);
        }

    }
}
