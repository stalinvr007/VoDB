using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VODB.VirtualDataBase;

namespace VODB.DbLayer.Loaders.TypeConverter
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
            return true;
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void SetValue(Object entity, Field field, Object value)
        {
            field.SetValue(entity, value);
        }
    }
}
