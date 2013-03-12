using System;
using VODB.Infrastructure;
using Fasterflect;
using VODB.Annotations;

namespace VODB.EntityTranslation
{
    public class EntityTranslator : IEntityTranslator
    {

        /// <summary>
        /// Translates the specified entity type.
        /// </summary>
        /// <param name="entityType">Type of entity.</param>
        /// <returns></returns>
        public ITable Translate(Type entityType)
        {
            var dbTable = entityType.Attribute<DbTableAttribute>();
            return new Table(dbTable != null ? dbTable.TableName : entityType.Name);
        }

    }
}
