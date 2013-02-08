using System;
using System.Collections.Generic;
using VODB.Core.Infrastructure;

namespace VODB.Core.Loaders
{
    internal interface ICachedEntity
    {
        Object Entity { get; }

        Object GetKeyValue(Field key);

        void Add(Field key, Object value);
    }

    internal class CachedEntity : ICachedEntity
    {
        private readonly IDictionary<Field, Object> originalKeyValues = new Dictionary<Field, object>();

        public CachedEntity(Object entity)
        {
            Entity = entity;
        }

        #region Implementation of ICachedEntity

        public object Entity { get; private set; }

        public object GetKeyValue(Field key)
        {
            Object value;
            return originalKeyValues.TryGetValue(key, out value)
                       ? value
                       : key.GetValue(Entity);
        }

        public void Add(Field key, object value)
        {
            originalKeyValues[key] = value;
        }

        #endregion
    }
}