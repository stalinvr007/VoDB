using System.Data.Common;
using VODB.Core;

namespace VODB.Core.Loaders
{

    internal class EntityKeyLoader : EntityLoader
    {
        public override void Load<TEntity>(TEntity entity, DbDataReader reader)
        {
            if (entity == null) return;
            
            foreach (var field in entity.GetTable().KeyFields)
            {
                SetValue(entity, field, GetValue(reader, field.FieldName), reader);
            }
        }
    }
}