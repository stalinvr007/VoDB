using System;
using VODB.Core.Infrastructure;

namespace VODB.Core.Loaders.FieldSetters
{
    /// <summary>
    /// Sets the field value. 
    /// </summary>
    internal sealed class BasicFieldSetter : IFieldSetter
    {
        #region IFieldSetter Members

        /// <summary>
        /// Determines whether this instance can handle the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public Boolean CanHandle(Type type)
        {
            return type.IsPrimitive ||
                   typeof (String).IsAssignableFrom(type) ||
                   typeof (DateTime).IsAssignableFrom(type) ||
                   typeof (Decimal).IsAssignableFrom(type) ||
                   typeof (Byte[]).IsAssignableFrom(type) ||
                   typeof (Guid).IsAssignableFrom(type);
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="session"></param>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <param name="getValueFromReader">The get value from reader.</param>
        public void SetValue<TEntity>(TEntity entity, IInternalSession session, Field field, Object value,
                                      Func<Field, Object> getValueFromReader)
        {
            if (value != DBNull.Value)
            {
                field.SetValue(entity, value);
            }
        }

        #endregion
    }
}