using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
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

        Type _EntityType;

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

            var thread = new Thread(() =>
            {
                var t1 = new Thread(() => table.TableName = GetTableName(_EntityType));
                var t2 = new Thread(() => table.Fields = GetTableFields(_EntityType));
                var t3 = new Thread(() => table.KeyFields = GetTableKeyFields(_EntityType));

                t3.Start();
                t2.Start();
                t1.Start();                

                t1.Join();
                t2.Join();
                t3.Join();
            });

            thread.Start();
            thread.Join();

            return table;
        }

        private static String GetTableName(Type type)
        {
            var tableAttr = type.GetCustomAttributes(typeof(DbTableAttribute), true).FirstOrDefault() as DbTableAttribute;

            return tableAttr == null ? type.Name : tableAttr.TableName;
        }

        private static IEnumerable<Field> GetTableFields(Type type)
        {

            return type.GetProperties()
                .Select(i => new Field
                {
                    FieldName = GetFieldName(i),
                    FieldType = i.PropertyType,
                    IsKey = IsKeyField(i)
                })
                .ToList();

        }

        private static Boolean IsKeyField(PropertyInfo info)
        {
            return info.GetAttribute<DbKeyAttribute>() != null ||
                info.GetAttribute<DbIdentityAttribute>() != null;
        }

        private static String GetFieldName(PropertyInfo info)
        {

            var dbField = info.GetAttribute<DbFieldAttribute>();

            if (dbField != null && !String.IsNullOrEmpty(dbField.FieldName))
            {
                return dbField.FieldName;
            }

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
