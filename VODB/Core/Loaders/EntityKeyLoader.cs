using System.Data.Common;
using VODB.Core;

namespace VODB.Core.Loaders
{

    internal class EntityKeyLoader : EntityLoader
    {
        public override void Load<TEntity>(TEntity entity, IInternalSession session, DbDataReader reader)
        {
            if (entity == null) return;
            
            foreach (var field in entity.GetTable().KeyFields)
            {
                SetValue(entity, session, field, GetValue(reader, field.FieldName), reader);
            }
        }
    }
}