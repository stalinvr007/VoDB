using System;
using VODB.Infrastructure;

namespace VODB.EntityTranslation
{
    /// <summary>
    /// Translates an entity type into a <see cref="VODB.Infrastructure.ITable"/> representation Type.
    /// </summary>
    public interface IEntityTranslator
    {

        /// <summary>
        /// Translates the specified entity type.
        /// </summary>
        /// <param name="entityType">Type of entity.</param>
        /// <returns></returns>
        ITable Translate(Type entityType);

    }
}
