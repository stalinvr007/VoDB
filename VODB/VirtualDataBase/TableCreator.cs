using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VODB.Annotations;

namespace VODB.VirtualDataBase
{
    /// <summary>
    /// Creates Tables with fields.
    /// Convention based and property Annotations to find out the table fields of an Entity.
    /// </summary>
    internal sealed class TableCreator : ITableCreator
    {
        readonly Type _EntityType;

        /// <summary>
        /// Initializes a new instance of the <see cref="TableCreator" /> class.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        public TableCreator(Type entityType)
        {
            _EntityType = entityType;
        }

        public Table Create()
        {
            var table = new Table();

            var threads = new ThreadCollection(
                () => table.TableName = GetTableName(_EntityType),
                () => table.Fields = GetTableFields(_EntityType),
                () => table.KeyFields = GetTableKeyFields(_EntityType));

            threads.StartAll();
            threads.JoinAll();

            return table;
        }

        private static String GetTableName(Type type)
        {
            var tableAttr = type.GetCustomAttributes(typeof(DbTableAttribute), true).FirstOrDefault() as DbTableAttribute;

            return tableAttr == null ? type.Name : tableAttr.TableName;
        }

        private static IEnumerable<Field> GetTableFields(Type type)
        {
            return type.GetProperties().Select(GetField).ToList();
        }

        private static Field GetField(PropertyInfo info)
        {
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

        private static IEnumerable<Field> GetTableKeyFields(Type type)
        {
            return GetTableFields(type).Where(f => f.IsKey);
        }



    }
}
