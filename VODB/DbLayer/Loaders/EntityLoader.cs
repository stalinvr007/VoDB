using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using VODB.DbLayer.Exceptions;
using VODB.VirtualDataBase;
using VODB.Extensions;

namespace VODB.DbLayer.Loaders
{
    /// <summary>
    /// Loads data into an entity from a DataReader.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    internal abstract class EntityLoader<TModel>
        where TModel : DbEntity, new()
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

            try
            {
                return reader[fieldName];
            }
            catch (Exception ex)
            {
                var table = reader.GetSchemaTable();
                if (!table.Columns.Contains(fieldName))
                {
                    throw new FieldNotFoundException(fieldName, table.TableName, ex);
                }
                throw ex;
            }

        }

        /// <summary>
        /// Sets the field value.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        protected Field SetValue(TModel entity, Field field, object value, DbDataReader reader)
        {
            foreach (var setter in Configuration.FieldSetters)
            {
                if (setter.CanHandle(field.FieldType))
                {
                    setter.SetValue(entity, field, value, (f) => GetValue(reader, f.FieldName));
                    break;
                }
            }

            return field;
        } 

        #endregion

        /// <summary>
        /// Loads the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public abstract void Load(TModel entity, DbDataReader reader);

    }


}
