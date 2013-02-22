using System.Data;
using VODB.Core.Infrastructure;

namespace VODB.Core.Loaders
{
    internal class EntityKeyLoader : EntityLoader
    {
        public EntityKeyLoader(ICachedEntities cache) : base(cache)
        {
        }

        protected override void LoadEntity<TEntity>(TEntity entity, IInternalSession session, IDataReader reader)
        {
            foreach (Field field in entity.GetTable().KeyFields)
            {
                SetValue(entity, session, field, GetValue(reader, field.FieldName), reader);
            }
        }
    }
}