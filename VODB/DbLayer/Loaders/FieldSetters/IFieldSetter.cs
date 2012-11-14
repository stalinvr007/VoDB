using System;
using VODB.VirtualDataBase;

namespace VODB.DbLayer.Loaders.FieldSetters
{
    /// <summary>
    /// Represents a field value setter.
    /// </summary>
    public interface IFieldSetter
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
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <param name="getValueFromReader">The get value from reader.</param>
        void SetValue(Object entity, Field field, Object value, Func<Field, Object> getValueFromReader);

    }
}
