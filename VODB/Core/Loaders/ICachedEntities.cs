using System;
using System.Collections.Generic;

namespace VODB.Core.Loaders
{
    internal interface ICachedEntities
    {
        ICachedEntity Add(Object entity);

        ICachedEntity Get(Object entity);
    }

    internal class CachedEntities : ICachedEntities
    {
        private readonly IDictionary<Object, ICachedEntity> entities = new Dictionary<Object, ICachedEntity>();

        #region Implementation of ICachedEntities

        public ICachedEntity Add(object entity)
        {
            return entities[entity] = new CachedEntity(entity);
        }

        public ICachedEntity Get(object entity)
        {
            ICachedEntity cached;
            return entities.TryGetValue(entity, out cached)
                       ? cached
                       : new CachedEntity(entity);
        }

        #endregion
    }
}