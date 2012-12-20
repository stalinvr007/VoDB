using System;
using VODB.Exceptions;
using VODB.Core.Infrastructure;
using VODB.Extensions;
using VODB.Core;

namespace VODB.Core.Loaders.FieldSetters
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
            return Engine.IsMapped(type);
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="session"></param>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <param name="getValueFromReader">The get value from reader.</param>
        public void SetValue<TEntity>(TEntity entity, ISession session, Field field, Object value, Func<Field, Object> getValueFromReader)
        {
            var foreignEntity = CreateInstance(field.FieldType);

            var table = Engine.GetTable(field.FieldType);

            /* Attempts to fill the key fields for this foreignEntity. */
            foreach (var key in table.KeyFields)
            {
                if (key.FieldName.Equals(field.BindedTo, StringComparison.InvariantCultureIgnoreCase) || 
                    key.FieldName.Equals(field.FieldName, StringComparison.InvariantCultureIgnoreCase))
                {
                    foreignEntity.SetValue(session, key, value, getValueFromReader);
                }
                else
                {
                    /* Have to search the entity for a field bindedTo this Key. Or with the same name. */
                    /* Use the name of that field to use on GetValueFromReader. */

                    var origField = entity.FindField(key.FieldName);
                    if (origField != null)
                    {
                        foreignEntity.SetValue(session, key, getValueFromReader(origField), getValueFromReader);
                    }
                }
            }

            field.SetValue(entity, foreignEntity);

        }

        private static Object CreateInstance(Type type)
        {
            try
            {
                return Activator.CreateInstance(type);
            }
            catch (Exception ex)
            {
                throw new UnableToInstantiateTypeException(type, ex);
            }
        }

    }
}
