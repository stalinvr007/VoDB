using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using VODB.Core;
using VODB.Core.Infrastructure;
using VODB.Core.Loaders;
using VODB.Core.Loaders.Factories;
using VODB.EntityValidators;
using VODB.Exceptions;
using VODB.ExpressionParser;

namespace VODB
{

    internal static class InternalExtensions
    {
        public static void Handle(this Exception ex)
        {
            var handler = Engine.Configuration.ExceptionHandlers.FirstOrDefault(eh => eh.CanHandle(ex));

            if (handler != null)
            {
                handler.Handle(ex);
            }
            else
            {
                throw ex;
            }
        }

        public static Table GetTable<TEntity>(this TEntity entity)
        {
            return Engine.GetTable(entity.GetType());
        }

        public static Boolean IsEntity(this Type entityType)
        {
            return Engine.IsMapped(entityType);
        }

        public static TEntity Make<TEntity>(this IEntityFactory factory, IInternalSession session)
        {
            return (TEntity)factory.Make(typeof(TEntity), session);
        }

        public static void SetParameters(this DbCommand cmd, IEnumerable<KeyValuePair<Key, Object>> collection)
        {
            foreach (var data in collection)
            {
                cmd.SetParameter(data.Key.Field, data.Key.ParamName, data.Value);
            }
        }

    }

    internal static class ConfigurationHelpers
    {

        static IConfiguration Configuration = Engine.Get<IConfiguration>();

        /// <summary>
        /// Gets the field by name.
        /// </summary>
        /// <param name="fields">The fields.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns></returns>
        public static Field GetFieldByName(this IEnumerable<Field> fields, String fieldName)
        {
            return fields.FirstOrDefault(f => f.FieldName.Equals(fieldName, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Validates the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="onCommand">The on command.</param>
        public static void ValidateEntity<TEntity>(this TEntity entity, On onCommand)
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
        public static void SetValue(this DbParameter param, Field field, Object entity)
        {
            try
            {
                var value = Engine.IsMapped(entity.GetType())
                            ? field.GetValue(entity)
                            : entity;

                param.SetParamValue(field, value);
            }
            catch (Exception ex)
            {
                throw new UnableToSetParameterValueException(ex, field.Table.TableName, field, entity);
            }
            

            
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="param">The param.</param>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="ParameterSetterNotFoundException"></exception>
        public static void SetParamValue(this DbParameter param, Field field, Object value)
        {
            var type = value == null ? field.FieldType : value.GetType();
            foreach (var setter in Configuration.ParameterSetters
                .Where(setter => setter.CanHandle(type)))
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
        /// <param name="session"></param>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        /// <exception cref="FieldSetterNotFoundException"></exception>
        /// <exception cref="FieldNotFoundException"></exception>
        public static Field SetValue<TModel>(this TModel entity, IInternalSession session, Field field, object value, DbDataReader reader)
        {
            if (value == null || value == DBNull.Value)
            {
                return field;
            }

            foreach (var setter in Configuration.FieldSetters
                .Where(setter => setter.CanHandle(field.FieldType)))
            {
                setter.SetValue(entity, session, field, value, f => reader.GetValue(f.FieldName));
                return field;
            }

            throw new FieldSetterNotFoundException(field.FieldType);
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="session"></param>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <param name="getValueFromReader">The get value from reader.</param>
        /// <returns></returns>
        /// <exception cref="FieldSetterNotFoundException"></exception>
        public static void SetValue<TModel>(this TModel entity, IInternalSession session, Field field, object value, Func<Field, object> getValueFromReader)
        {
            if (value == null || value == DBNull.Value)
            {
                return;
            }

            foreach (var setter in Configuration.FieldSetters
                .Where(setter => setter.CanHandle(field.FieldType)))
            {
                setter.SetValue(entity, session, field, value, getValueFromReader);
                return;
            }

            throw new FieldSetterNotFoundException(field.FieldType);

        }
    }

    internal static class FieldHelpers
    {
        static readonly IConfiguration Configuration = Engine.Get<IConfiguration>();

        public static Field FindField(this Table table, String BindOrName)
        {
            return table.FindField(BindOrName);
        }

        /// <summary>
        /// Determines whether the specified entity is filled.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="field">The field.</param>
        /// <returns></returns>
        /// <exception cref="FieldValidatorNotFoundException"></exception>
        public static Boolean IsFilled<TEntity>(this TEntity entity, Field field)
        {
            foreach (var validator in Configuration.FieldIsFilledValidators.Where(validator => validator.CanHandle(field)))
            {
                return validator.Verify(field, entity);
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
                lock (reader)
                {
                    return reader[fieldName];    
                }
            }
            catch (Exception ex)
            {
                throw new FieldNotFoundException(fieldName, "", ex);
            }
        }

        /// <summary>
        /// Sets the parameter.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="dbCommand">The db command.</param>
        /// <param name="field">The field.</param>
        /// <param name="entity">The entity.</param>
        /// <exception cref="ParameterSetterNotFoundException"></exception>
        private static void SetParameter<TEntity>(this DbCommand dbCommand, Field field, TEntity entity)
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
        /// <param name="dbCommand">The db command.</param>
        /// <param name="cache">The cache.</param>
        /// <param name="field">The field.</param>
        /// <exception cref="ParameterSetterNotFoundException"></exception>
        private static void SetOldParameter(this DbCommand dbCommand, ICachedEntity cache, Field field)
        {
            var param = dbCommand.CreateParameter();
            param.ParameterName = string.Format("Old{0}", field.FieldName);
            param.SetValue(field, cache.GetKeyValue(field));

            dbCommand.Parameters.Add(param);
        }


        /// <summary>
        /// Sets the parameters.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="dbCommand">The db command.</param>
        /// <param name="fields">The fields.</param>
        /// <param name="entity">The entity.</param>
        /// <exception cref="ParameterSetterNotFoundException"></exception>
        public static void SetParameters<TEntity>(this DbCommand dbCommand, IEnumerable<Field> fields, TEntity entity)
        {
            foreach (var field in fields)
            {
                dbCommand.SetParameter(field, entity);
            }
        }

        /// <summary>
        /// Sets the old parameters.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="dbCommand">The db command.</param>
        /// <param name="table">The table.</param>
        /// <param name="entity">The entity.</param>
        /// <exception cref="ParameterSetterNotFoundException"></exception>
        public static void SetOldParameters<TEntity>(this DbCommand dbCommand, Table table, TEntity entity)
        {
            var cache = Engine.Get<ICachedEntities>().Get(entity);
            foreach (var field in table.KeyFields)
            {
                dbCommand.SetOldParameter(cache, field);
            }
        }
    }
}
