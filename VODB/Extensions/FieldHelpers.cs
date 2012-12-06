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

        /// <summary>
        /// Validates the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="onCommand">The on command.</param>
        public static void ValidateEntity(this Entity entity, On onCommand)
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
        /// <exception cref="ParameterSetterNotFoundException"></exception>
        public static void SetValue(this DbParameter param, Field field, Entity entity)
        {
            param.SetValue(field, field.GetValue(entity));
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="param">The param.</param>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="ParameterSetterNotFoundException"></exception>
        public static void SetValue(this DbParameter param, Field field, Object value)
        {
            foreach (var setter in Configuration.ParameterSetters
                .Where(setter => setter.CanHandle(field.FieldType)))
            {

                try
                {
                    setter.SetValue(param, field, value);
                }

                catch (Exception ex)
                {
                    throw new UnableToSetParameterValueException(ex, field.Table.TableName, field, value);
                }
                
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
        /// <exception cref="FieldSetterNotFoundException"></exception>
        /// <exception cref="FieldNotFoundException"></exception>
        public static Field SetValue<TModel>(this TModel entity, Field field, object value, DbDataReader reader)
            where TModel : Entity
        {
            if (value == null || value == DBNull.Value)
            {
                return field;
            }

            foreach (var setter in Configuration.FieldSetters
                .Where(setter => setter.CanHandle(field.FieldType)))
            {
                setter.SetValue(entity, field, value, f => reader.GetValue(f.FieldName));
                return field;
            }

            throw new FieldSetterNotFoundException(field.FieldType);
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
        /// <exception cref="FieldSetterNotFoundException"></exception>
        public static void SetValue<TModel>(this TModel entity, Field field, object value, Func<Field, object> getValueFromReader)
            where TModel : Entity
        {
            if (value == null || value == DBNull.Value)
            {
                return;
            }

            foreach (var setter in Configuration.FieldSetters
                .Where(setter => setter.CanHandle(field.FieldType)))
            {
                setter.SetValue(entity, field, value, getValueFromReader);
                return;
            }

            throw new FieldSetterNotFoundException(field.FieldType);

        }
    }

    internal static class FieldHelpers
    {

        public static Field FindField(this Entity entity, String BindOrName)
        {

            return entity.Table.Fields
                .FirstOrDefault(field =>
                 {
                     if (field.FieldName.Equals(BindOrName, StringComparison.InvariantCultureIgnoreCase))
                     {
                         return true;
                     }

                     if (!String.IsNullOrEmpty(field.BindedTo) && field.BindedTo.Equals(BindOrName, StringComparison.InvariantCultureIgnoreCase))
                     {
                         return true;
                     }

                     return false;
                 });

        }

        /// <summary>
        /// Determines whether the specified entity is filled.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="field">The field.</param>
        /// <returns></returns>
        /// <exception cref="FieldValidatorNotFoundException"></exception>
        public static Boolean IsFilled(this Entity entity, Field field)
        {
            foreach (var validator in Configuration.FieldIsFilledValidators)
            {
                if (validator.CanHandle(field))
                {
                    return validator.Verify(field, entity);
                }
            }

            throw new FieldValidatorNotFoundException(field.FieldType);
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
                if (table != null && !table.Columns.Contains(fieldName))
                {
                    throw new FieldNotFoundException(fieldName, table.TableName, ex);
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Sets the parameter.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="dbCommand">The db command.</param>
        /// <param name="field">The field.</param>
        /// <param name="entity">The entity.</param>
        /// <exception cref="ParameterSetterNotFoundException"></exception>
        private static void SetParameter<TModel>(this DbCommand dbCommand, Field field, TModel entity)
            where TModel : Entity
        {
            var param = dbCommand.CreateParameter();
            param.ParameterName = field.FieldName;
            param.SetValue(field, entity);

            dbCommand.Parameters.Add(param);
        }

        /// <summary>
        /// Sets the parameter.
        /// </summary>
        /// <param name="dbCommand">The db command.</param>
        /// <param name="field">The field.</param>
        /// <param name="paramName">Name of the param.</param>
        /// <param name="value">The value.</param>
        public static void SetParameter(this DbCommand dbCommand, Field field, String paramName, Object value)
        {
            var param = dbCommand.CreateParameter();
            param.ParameterName = paramName;
            param.SetValue(field, value);

            dbCommand.Parameters.Add(param);
        }

        /// <summary>
        /// Sets the parameter.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="dbCommand">The db command.</param>
        /// <param name="field">The field.</param>
        /// <param name="entity">The entity.</param>
        /// <exception cref="ParameterSetterNotFoundException"></exception>
        private static void SetOldParameter<TModel>(this DbCommand dbCommand, Field field, TModel entity)
            where TModel : Entity
        {
            var param = dbCommand.CreateParameter();
            param.ParameterName = string.Format("Old{0}", field.FieldName);
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
        /// <exception cref="ParameterSetterNotFoundException"></exception>
        public static void SetParameters<TModel>(this DbCommand dbCommand, IEnumerable<Field> fields, TModel entity)
            where TModel : Entity
        {
            foreach (Field field in fields)
            {
                dbCommand.SetParameter(field, entity);
            }
        }

        /// <summary>
        /// Sets the old parameters.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="dbCommand">The db command.</param>
        /// <param name="entity">The entity.</param>
        /// <exception cref="ParameterSetterNotFoundException"></exception>
        public static void SetOldParameters<TModel>(this DbCommand dbCommand, TModel entity)
            where TModel : Entity
        {
            foreach (var field in entity.Table.KeyFields)
            {
                dbCommand.SetOldParameter(field, entity);
            }
        }
    }
}