﻿using System;
using System.Data.Common;
using VODB.Extensions;
using VODB.Core.Infrastructure;

namespace VODB.Core.Loaders
{
    /// <summary>
    /// Loads data into an entity from a DataReader.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    internal abstract class EntityLoader : IEntityLoader
    {
        #region FIELD GETTER SETTER

        /// <summary>
        /// Gets the field value from the datareader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        protected object GetValue(DbDataReader reader, String fieldName)
        {
            return reader.GetValue(fieldName);
        }

        /// <summary>
        /// Sets the field value.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <param name="reader"> </param>
        /// <returns></returns>
        protected void SetValue<TEntity>(TEntity entity, IInternalSession session, Field field, object value, DbDataReader reader)
        {
            if (field.IsKey)
            {
                // inEntity.AddKeyOriginalValue(field, value);
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
        public abstract void Load<TEntity>(TEntity entity, IInternalSession session, DbDataReader reader);
    }
}