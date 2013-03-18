using System;
using VODB.Infrastructure;
using Fasterflect;
using VODB.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using VODB.Exceptions;
using System.Collections;
using VODB.TableToSql;

namespace VODB.EntityTranslation
{

    class EntityTranslator : IEntityTranslator
    {

        private static IDictionary<Type, ITable> tables = new Dictionary<Type, ITable>();

        private static IList<Type> fieldAttributes = new List<Type>()
        {
            typeof(DbFieldAttribute),
            typeof(DbKeyAttribute),
            typeof(DbIdentityAttribute)
        };

        private static IDictionary<SqlBuilderType, ISqlBuilder> builders = new Dictionary<SqlBuilderType, ISqlBuilder>()
        {
            { SqlBuilderType.CountById, new CountByIdBuilder() },
            { SqlBuilderType.Count, new CountBuilder() },
            { SqlBuilderType.Delete, new DeleteByIdBuilder() },
            { SqlBuilderType.Insert, new InsertBuilder() },
            { SqlBuilderType.Select, new SelectBuilder() },
            { SqlBuilderType.SelectById, new SelectByIdBuilder() },
            { SqlBuilderType.Update, new UpdateBuilder() }
        };



        public ITable Translate(Type entityType)
        {
            var dbTable = entityType.Attribute<DbTableAttribute>();

            var table = new Table(dbTable != null ? dbTable.TableName : entityType.Name);

            table.EntityType = entityType;

            table.Fields = MakeFields(entityType, table);

            table.Fields = table.Fields.Select(f => SetFieldBindMember(this, f.Info, (Field)f)).ToList();

            table.Keys = table.Fields
                .Where(f => f.IsKey).ToList();

            Parallel.Invoke(

                () => table.SqlCount = builders[SqlBuilderType.Count].Build(table),
                () => table.SqlCountById = builders[SqlBuilderType.CountById].Build(table),
                () => table.SqlDeleteById = builders[SqlBuilderType.Delete].Build(table),
                () => table.SqlInsert = builders[SqlBuilderType.Insert].Build(table),
                () => table.SqlSelect = builders[SqlBuilderType.Select].Build(table),
                () => table.SqlSelectById = builders[SqlBuilderType.SelectById].Build(table),
                () => table.SqlUpdate = builders[SqlBuilderType.Update].Build(table)

            );
                        
            return table;
        }

        private IList<IField> MakeFields(Type entityType, ITable table)
        {
            IList<IField> fields = new List<IField>();

            foreach (var item in entityType.GetProperties()
                .Where(pi => !pi.HasAttribute<DbIgnoreAttribute>())
                .Where(pi => !pi.PropertyType.IsGenericType || !pi.PropertyType.GetInterfaces().Contains(typeof(IEnumerable))))
            {
                Field field = MakeField(entityType, item);

                field.Info = item;
                field.Table = table;
                field.IsIdentity = item.HasAttribute<DbIdentityAttribute>();
                field.IsKey = field.IsIdentity || item.HasAttribute<DbKeyAttribute>();

                fields.Add(field);
            }

            return fields;
        }

        private static IField SetFieldBindMember(IEntityTranslator translator, PropertyInfo item, Field field)
        {
            var bind = item.Attribute<DbBindAttribute>();
            String bindFieldName = null;

            if (bind != null)
            {
                if (!item.GetMethod.IsVirtual)
                {
                    throw new InvalidMappingException("The field [{0}] is marked with DbBind but is not Virtual.", field.Name);
                }

                bindFieldName = bind.FieldName;
            }

            if (item.GetMethod.IsVirtual)
            {
                bindFieldName = bindFieldName ?? field.Name;

                var table = item.PropertyType != field.EntityType ?
                    translator.Translate(item.PropertyType) :
                    field.Table;

                field.BindToField = table.Fields
                    .FirstOrDefault(f => f.Name.Equals(bindFieldName, StringComparison.InvariantCultureIgnoreCase));

                if (field.BindToField == null)
                {
                    throw new InvalidMappingException("The field [{0}] is a reference to the table [{1}] but no match was found.", field.Name, table.Name);
                }

                return new BindedField(field);
            }

            return field;

        }

        private static Field MakeField(Type entityType, PropertyInfo item)
        {
            String fieldName = null;
            MemberSetter setter = null;
            MemberGetter getter = null;

            Parallel.Invoke(

                () => fieldName = GetFieldName(item),
                () => getter = item.DelegateForGetPropertyValue(),
                () => setter = item.DelegateForSetPropertyValue()

            );

            return new Field(fieldName, entityType, MakeValueSetter(fieldName, setter), MakeValueGetter(fieldName, getter));
        }

        private static Func<Object, Object> MakeValueGetter(String fieldName, MemberGetter getter)
        {
            return (entity) =>
            {
                try
                {
                    return getter(entity);
                }
                catch (Exception ex)
                {
                    throw new UnableToGetTheFieldValueException(ex, fieldName);
                }
            };
        }

        private static Action<Object, Object> MakeValueSetter(String fieldName, MemberSetter setter)
        {
            return (entity, value) =>
            {
                try
                {
                    setter(entity, value);
                }
                catch (Exception ex)
                {
                    throw new UnableToSetTheFieldValueException(ex, fieldName, value);
                }
            };
        }

        private static String GetFieldName(PropertyInfo property)
        {
            foreach (var item in fieldAttributes)
            {
                var attr = property.Attribute(item) as DbFieldBase;
                if (attr != null)
                {
                    return attr.FieldName ?? property.Name;
                }
            }

            return property.Name;
        }

    }
}
