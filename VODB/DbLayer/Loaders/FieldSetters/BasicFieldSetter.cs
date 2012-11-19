using System;
using VODB.VirtualDataBase;

namespace VODB.DbLayer.Loaders.FieldSetters
{
    /// <summary>
    /// Sets the field value. 
    /// </summary>
    public sealed class BasicFieldSetter : IFieldSetter
    {
        /// <summary>
        /// Determines whether this instance can handle the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public Boolean CanHandle(Type type)
        {
            return type.IsPrimitive || 
                typeof(String).IsAssignableFrom(type) ||
                typeof(DateTime).IsAssignableFrom(type) ||
                typeof(Byte[]).IsAssignableFrom(type);
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <param name="getValueFromReader">The get value from reader.</param>
        public void SetValue(DbEntity entity, Field field, Object value, Func<Field, Object> getValueFromReader)
        {
            if (value != DBNull.Value)
            {
                field.SetValue(entity, value);
            }
        }
    }
}
