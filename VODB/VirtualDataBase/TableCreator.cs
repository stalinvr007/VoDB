﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
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

            var t1 = new Task<String>(() => GetTableName(_EntityType));
            var t2 = new Task<IEnumerable<Field>>(() => GetTableFields(_EntityType));
            var t3 = new Task<IEnumerable<Field>>(() => GetTableKeyFields(_EntityType));
            var t4 = new Task<ITSqlCommandHolder>(() => new TSqlCommandHolder(table));
            
            t1.Start();
            t2.Start();
            t3.Start();
            
            table.TableName = t1.Result;
            table.Fields = t2.Result;
            table.KeyFields = t3.Result;

            t4.Start();
            table.CommandsHolder = t4.Result;

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
            
            if (info.GetCustomAttributes(true).Count() == 0)
            {
                return new Field(info)
                {
                    FieldName = info.Name,
                    FieldType = info.PropertyType,
                    IsKey = false,
                    IsIdentity = false,
                    BindedTo = GetBindedTo(info),
                    IsRequired = info.GetAttribute<DbRequiredAttribute>() != null
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
                    IsIdentity = false,
                    BindedTo = GetBindedTo(info),
                    IsRequired = info.GetAttribute<DbRequiredAttribute>() != null
                };
            }

            return new Field(info)
            {
                FieldName = GetKeyFieldName(info),
                FieldType = info.PropertyType,
                IsKey = IsKeyField(info),
                IsIdentity = IsIdentityField(info),
                BindedTo = GetBindedTo(info),
                IsRequired = info.GetAttribute<DbRequiredAttribute>() != null
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

        private static string GetBindedTo(PropertyInfo info)
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

        private static IEnumerable<Field> GetTableKeyFields(Type type)
        {
            return GetTableFields(type).Where(f => f.IsKey);
        }



    }
}
