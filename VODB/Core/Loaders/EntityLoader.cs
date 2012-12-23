using System;
using System.Data.Common;
using VODB.Core.Infrastructure;

namespace VODB.Core.Loaders
{
    /// <summary>
    /// Loads data into an entity from a DataReader.
    /// </summary>
    internal abstract class EntityLoader : IEntityLoader
    {
        private readonly ICachedEntities _cache;
        ICachedEntity cachedEntity;

        protected EntityLoader(ICachedEntities cache)
        {
            _cache = cache;
        }

        #region FIELD GETTER SETTER

        /// <summary>
        /// Gets the field value from the datareader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        protected static object GetValue(DbDataReader reader, String fieldName)
        {
            return reader.GetValue(fieldName);
        }

        /// <summary>
        /// Sets the field value.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="session">The session.</param>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <param name="reader">The reader.</param>
        protected void SetValue<TEntity>(TEntity entity, IInternalSession session, Field field, object value, DbDataReader reader)
        {
            if (field.IsKey)
            {
                if (!entity.Equals(cachedEntity.Entity))
                {
                    cachedEntity = _cache.Get(entity);
                }
                cachedEntity.Add(field, value);
            }
            entity.SetValue(session, field, value, reader);
        }

        #endregion

        /// <summary>
        /// Loads the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="session"></param>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public void Load<TEntity>(TEntity entity, IInternalSession session, DbDataReader reader)
        {
            cachedEntity = _cache.Add(entity);
            LoadEntity(entity, session, reader);
        }

        /// <summary>
        /// Loads the entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="session">The session.</param>
        /// <param name="reader">The reader.</param>
        protected abstract void LoadEntity<TEntity>(TEntity entity, IInternalSession session, DbDataReader reader);
    }
}