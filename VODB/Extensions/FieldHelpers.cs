using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using VODB.EntityValidators;
using VODB.Exceptions;
using VODB.VirtualDataBase;

namespace VODB.Extensions
{

    internal static class ConfigurationHelpers
    {

        public static void ValidateEntity(this DbEntity entity, On onCommand)
        {

            foreach (var validator in Configuration.EntityValidators
                .Where(val => val.ShouldRunOn(onCommand)))
            {
                validator.Validate(entity);
            }

        }

        /// <summary>
        /// Sets the parameter.
        /// </summary>
        /// <param name="param">The param.</param>
        /// <param name="field">The field.</param>
        /// <param name="entity">The entity.</param>
        public static void SetValue(this DbParameter param, Field field, Object entity)
        {
            foreach (var setter in Configuration.ParameterSetters
                .Where(setter => setter.CanHandle(field.FieldType)))
            {
                setter.SetValue(param, field, entity);
                return;
            }

            throw new ParameterSetterNotFoundException(field.FieldType);
        }

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
            if (value == null || value == DBNull.Value)
            {
                return field;
            }

            var handled = false;
            foreach (var setter in Configuration.FieldSetters
                .Where(setter => setter.CanHandle(field.FieldType)))
            {
                setter.SetValue(entity, field, value, f => reader.GetValue(f.FieldName));
                handled = true;
                break;
            }
            if (!handled)
            {
                throw new FieldSetterNotFoundException(field.FieldType);
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
        public static void SetValue<TModel>(this TModel entity, Field field, object value, Func<Field, object> getValueFromReader)
            where TModel : DbEntity
        {
            if (value == null || value == DBNull.Value)
            {
                return;
            }

            var handled = false;
            foreach (var setter in Configuration.FieldSetters
                .Where(setter => setter.CanHandle(field.FieldType)))
            {
                setter.SetValue(entity, field, value, getValueFromReader);
                handled = true;
                break;
            }
            if (!handled)
            {
                throw new FieldSetterNotFoundException(field.FieldType);
            }
        }
    }

    internal static class FieldHelpers
    {
        

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
                if (table != null && !table.Columns.Contains(fieldName))
                {
                    throw new FieldNotFoundException(fieldName, table.TableName, ex);
                }
                throw;
            }
        }

        /// <summary>
        /// Sets the parameter.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="dbCommand">The db command.</param>
        /// <param name="field">The field.</param>
        /// <param name="entity">The entity.</param>
        private static void SetParameter<TModel>(this DbCommand dbCommand, Field field, TModel entity)
            where TModel : DbEntity
        {
            var param = dbCommand.CreateParameter();
            param.ParameterName = field.FieldName;
            param.SetValue(field, entity);

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
            foreach (Field field in fields)
            {
                dbCommand.SetParameter(field, entity);
            }
        }
    }
}