using System;
using VODB.Exceptions;
using VODB.VirtualDataBase;
using VODB.Extensions;

namespace VODB.DbLayer.Loaders.FieldSetters
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
            return typeof(Entity).IsAssignableFrom(type);
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <param name="getValueFromReader">The get value from reader.</param>
        public void SetValue(Entity entity, Field field, Object value, Func<Field, Object> getValueFromReader)
        {
            var foreignEntity = CreateInstance(field.FieldType);

            /* Attempts to fill the key fields for this foreignEntity. */
            foreach (var key in foreignEntity.Table.KeyFields)
            {
                if (key.FieldName.Equals(field.BindedTo, StringComparison.InvariantCultureIgnoreCase) || 
                    key.FieldName.Equals(field.FieldName, StringComparison.InvariantCultureIgnoreCase))
                {
                    foreignEntity.SetValue(key, value, getValueFromReader);
                }
                else
                {
                    /* Have to search the entity for a field bindedTo this Key. Or with the same name. */
                    /* Use the name of that field to use on GetValueFromReader. */

                    var origField = entity.FindField(key.FieldName);
                    if (origField != null)
                    {
                        foreignEntity.SetValue(key, getValueFromReader(origField), getValueFromReader);
                    }
                }
            }

            field.SetValue(entity, foreignEntity);

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
