using System;
using VODB.Core.Infrastructure;

namespace VODB.Core.Loaders.FieldSetters
{
    /// <summary>
    /// Represents a field value setter.
    /// </summary>
    internal interface IFieldSetter
    {
        /// <summary>
        /// Determines whether this instance can handle the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        Boolean CanHandle(Type type);

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="session"></param>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <param name="getValueFromReader">The get value from reader.</param>
        void SetValue<TEntity>(TEntity entity, IInternalSession session, Field field, Object value,
                               Func<Field, Object> getValueFromReader);
    }
}