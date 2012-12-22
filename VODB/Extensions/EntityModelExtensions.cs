using System;
using System.Linq;
using VODB.Annotations;

namespace VODB.Extensions
{
    internal static class EntityModelExtensions
    {

        /// <summary>
        /// Determines whether the specified type is entity.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if the specified type is entity; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsEntity(Type type)
        {
            return type.GetCustomAttributes(typeof(DbTableAttribute), true).Any();
        }

        public static bool IsEntity<TEntity>()
        {
            return IsEntity(typeof (TEntity));
        }

    }
}
