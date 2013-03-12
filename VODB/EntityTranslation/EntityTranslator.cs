using System;
using VODB.Infrastructure;
using Fasterflect;
using VODB.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;

namespace VODB.EntityTranslation
{
    class EntityTranslator : IEntityTranslator
    {

        /// <summary>
        /// Translates the specified entity type.
        /// </summary>
        /// <param name="entityType">Type of entity.</param>
        /// <returns></returns>
        public ITable Translate(Type entityType)
        {
            var dbTable = entityType.Attribute<DbTableAttribute>();
            return new Table(
                dbTable != null ? dbTable.TableName : entityType.Name,
                GetFields(entityType));
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

                fields.Add(new Field(
                    fieldName,
                    entityType,
                    (entity, value) => setter(entity, value),
                    (entity) => getter(entity))
                );

            }

            return fields;
        }

        private String GetFieldName(PropertyInfo property)
        {
            return property.Name;

        }

        private Task<MemberSetter> GetFieldValueSetter(PropertyInfo property)
        {
            return Task<MemberSetter>.Factory.StartNew(() =>
            {
                return property.DelegateForSetPropertyValue();
            });
        }

        private Task<MemberGetter> GetFieldValueGetter(PropertyInfo property)
        {
            return Task<MemberGetter>.Factory.StartNew(() =>
            {
                return property.DelegateForGetPropertyValue();
            });
        }

    }
}
