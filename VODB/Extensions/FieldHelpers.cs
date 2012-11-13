using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using VODB.DbLayer.Exceptions;
using VODB.VirtualDataBase;

namespace VODB.Extensions
{
    internal static class FieldHelpers
    {

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public static Field SetValue<TModel>(this TModel entity, Field field, object value, DbDataReader reader)
            where TModel : DbEntity
        {
            foreach (var setter in Configuration.FieldSetters)
            {
                if (setter.CanHandle(field.FieldType))
                {
                    setter.SetValue(entity, field, value, (f) => reader.GetValue(f.FieldName));
                    break;
                }
            }

            return field;
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <param name="getValueFromReader">The get value from reader.</param>
        /// <returns></returns>
        public static Field SetValue<TModel>(this TModel entity, Field field, object value, Func<Field, Object> getValueFromReader)
            where TModel : DbEntity
        {
            foreach (var setter in Configuration.FieldSetters)
            {
                if (setter.CanHandle(field.FieldType))
                {
                    setter.SetValue(entity, field, value, getValueFromReader);
                    break;
                }
            }

            return field;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns></returns>
        /// <exception cref="FieldNotFoundException"></exception>
        public static object GetValue(this DbDataReader reader, String fieldName)
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
        /// Sets the parameter.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="dbCommand">The db command.</param>
        /// <param name="field">The field.</param>
        /// <param name="entity">The entity.</param>
        public static void SetParameter<TModel>(this DbCommand dbCommand, Field field, TModel entity)
            where TModel : DbEntity
        {
            var param = dbCommand.CreateParameter();
            param.ParameterName = field.FieldName;
            param.Value = field.GetValue(entity);
            dbCommand.Parameters.Add(param);
        }

        /// <summary>
        /// Sets the parameters.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="dbCommand">The db command.</param>
        /// <param name="fields">The fields.</param>
        /// <param name="entity">The entity.</param>
        public static void SetParameters<TModel>(this DbCommand dbCommand, IEnumerable<Field> fields, TModel entity)
             where TModel : DbEntity
        {
            foreach (var field in fields)
            {
                dbCommand.SetParameter(field, entity);
            }
        }
    }
}
