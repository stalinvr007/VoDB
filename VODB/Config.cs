using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using VODB.Core;

namespace VODB
{
    public static class Config
    {

        /// <summary>
        /// Maps TEntity as a VODB entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        public static void Map<TEntity>()
        {
            Engine.Map<TEntity>();
        }

        /// <summary>
        /// Maps the specified name space.
        /// </summary>
        /// <param name="nameSpace">The name space.</param>
        public static void Map(String nameSpace)
        {
            foreach (var type in Assembly.GetCallingAssembly().GetTypes()
                .Where(t => !String.IsNullOrEmpty(t.Namespace))
                .Where(t => t.Namespace.Equals(nameSpace)))
            {
                Engine.Map(type);
            }
        }

    }
}
