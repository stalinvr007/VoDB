using System;
using VODB.Infrastructure;
using Fasterflect;
using VODB.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using VODB.Exceptions;

namespace VODB.EntityTranslation
{
    class EntityTranslator : IEntityTranslator
    {

        static IList<Type> fieldAttributes = new List<Type>()
        {
            typeof(DbFieldAttribute),
            typeof(DbKeyAttribute),
            typeof(DbIdentityAttribute)
        };

        public ITable Translate(Type entityType)
        {
            var dbTable = entityType.Attribute<DbTableAttribute>();
            var fields = GetFields(entityType);

            return new Table(
                dbTable != null ? dbTable.TableName : entityType.Name,
                fields, 
                fields.Where(f => f.IsKey).ToList()
            );
        }

        private IList<IField> GetFields(Type entityType)
        {
            IList<IField> fields = new List<IField>();

            foreach (var item in entityType.GetProperties().AsParallel())
            {
                String fieldName = null;
                MemberSetter setter = null;
                MemberGetter getter = null;

                Parallel.Invoke(

                    () => fieldName = GetFieldName(item),
                    () => getter = item.DelegateForGetPropertyValue(),
                    () => setter = item.DelegateForSetPropertyValue()

                );

                var field = new Field(fieldName, entityType, MakeValueSetter(fieldName, setter), MakeValueGetter(fieldName, getter));

                field.IsIdentity = item.HasAttribute<DbIdentityAttribute>();
                field.IsKey = field.IsIdentity || item.HasAttribute<DbKeyAttribute>();

                fields.Add(field);
            }

            return fields;
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

        private String GetFieldName(PropertyInfo property)
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
