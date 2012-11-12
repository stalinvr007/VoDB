using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VODB.VirtualDataBase;
using VODB.DbLayer.Exceptions;

namespace VODB.DbLayer.Loaders.TypeConverter
{
    /// <summary>
    /// Sets the value for a DbEntity type field.
    /// </summary>
    public sealed class DbEntityFieldSetter : IFieldSetter
    {

        /// <summary>
        /// Determines whether this instance can handle the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public Boolean CanHandle(Type type)
        {
            return typeof(DbEntity).IsAssignableFrom(type);
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <param name="getValueFromReader">The get value from reader.</param>
        public void SetValue(Object entity, Field field, Object value, Func<Field, Object> getValueFromReader)
        {
            var foreignEntity = CreateInstance(field.FieldType);

            /* Attempts to fill the key fields for this foreignEntity. */
            foreach (var key in foreignEntity.Table.KeyFields)
            {
                if (key.FieldName.Equals(field.BindedTo) || key.FieldName.Equals(field.FieldName))
                {
                    InternalSetValue(foreignEntity, key, value, getValueFromReader);
                }
                else
                {
                    InternalSetValue(foreignEntity, key, getValueFromReader(key), getValueFromReader);
                }
            }
            
            field.SetValue(entity, foreignEntity);
        }

        private static void InternalSetValue(Object entity, Field field, Object value, Func<Field, Object> getValueFromReader)
        {
            foreach (var setter in Configuration.FieldSetters)
            {
                if (setter.CanHandle(field.FieldType))
                {
                    setter.SetValue(entity, field, value, getValueFromReader);
                    break;
                }
            }
        }

        private static DbEntity CreateInstance(Type type)
        {
            try
            {
                return Activator.CreateInstance(type) as DbEntity;
            }
            catch (Exception ex)
            {
                throw new UnableToInstantiateTypeException(type, ex);
            }            
        }

    }
}
