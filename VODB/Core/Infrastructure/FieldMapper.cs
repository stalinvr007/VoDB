using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VODB.Annotations;
using VODB.Core.Infrastructure;
using VODB.Extensions;
using System.Reflection;
using System.Collections;

namespace VODB.Core.Infrastructure
{

    interface IFieldMapper
    {
        IEnumerable<Field> GetFields(Type entityType);
    }

    interface IFieldMapper<TEntity>
    {
        IEnumerable<Field> GetFields();
    }

    class FieldMapper : IFieldMapper
    {
        #region Static Auxiliary Functions

        private static String GetFieldName(DbFieldAttribute dbField, PropertyInfo info)
        {
            if (dbField != null && !String.IsNullOrEmpty(dbField.FieldName))
            {
                return dbField.FieldName;
            }

            return info.Name;
        }

        private static Boolean IsIdentityField(PropertyInfo info)
        {
            return info.GetAttribute<DbIdentityAttribute>() != null;
        }

        private static Boolean IsKeyField(PropertyInfo info)
        {
            return info.GetAttribute<DbKeyAttribute>() != null ||
                info.GetAttribute<DbIdentityAttribute>() != null;
        }

        private static String GetBindedTo(PropertyInfo info)
        {
            var bind = info.GetAttribute<DbBindAttribute>();
            return bind != null ? bind.FieldName : null;
        }

        private static String GetKeyFieldName(PropertyInfo info)
        {

            var dbKey = info.GetAttribute<DbKeyAttribute>();

            if (dbKey != null && !String.IsNullOrEmpty(dbKey.FieldName))
            {
                return dbKey.FieldName;
            }

            var dbIdentity = info.GetAttribute<DbIdentityAttribute>();

            if (dbIdentity != null && !String.IsNullOrEmpty(dbIdentity.FieldName))
            {
                return dbIdentity.FieldName;
            }

            return info.Name;
        }

        private static Field SetCommunSettings(Field field, PropertyInfo info)
        {
            field.BindedTo = GetBindedTo(info);
            field.IsRequired = (field.IsKey && !field.IsIdentity) || info.GetAttribute<DbRequiredAttribute>() != null;
            return field;
        }

        private static Field GetField(PropertyInfo info)
        {

            if (!info.GetCustomAttributes(true).Any())
            {
                return new Field(info)
                {
                    FieldName = info.Name,
                    FieldType = info.PropertyType,
                    IsKey = false,
                    IsIdentity = false
                };
            }

            var dbField = info.GetAttribute<DbFieldAttribute>();
            if (dbField != null)
            {
                return new Field(info)
                {
                    FieldName = GetFieldName(dbField, info),
                    FieldType = info.PropertyType,
                    IsKey = false,
                    IsIdentity = false
                };
            }

            return new Field(info)
            {
                FieldName = GetKeyFieldName(info),
                FieldType = info.PropertyType,
                IsKey = IsKeyField(info),
                IsIdentity = IsIdentityField(info)
            };
        }

        #endregion

        #region IFieldMapping<TEntity> Implementation
        
        public IEnumerable<Field> GetFields(Type entityType)
        {
            return entityType.GetProperties()
                .Where(info => !info.PropertyType.IsGenericType)
                .Where(info => info.GetAttribute<DbIgnoreAttribute>() == null)
                .Select(info => SetCommunSettings(GetField(info), info));
        }

        #endregion
    }

    class FieldMapper<TEntity> : IFieldMapper<TEntity>
    {

        private readonly IFieldMapper _FieldMapper;

        public FieldMapper(IFieldMapper fieldMapper)
        {
            _FieldMapper = fieldMapper;
        }

        #region IFieldMapping<TEntity> Implementation

        public virtual IEnumerable<Field> GetFields()
        {
            return _FieldMapper.GetFields(typeof(TEntity));
        } 

        #endregion
    }

}
