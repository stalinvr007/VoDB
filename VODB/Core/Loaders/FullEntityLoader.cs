using System.Data.Common;

namespace VODB.Core.Loaders
{
    /// <summary>
    /// Loads all the data to the entity from the DataReader.
    /// </summary>
    internal class FullEntityLoader: EntityLoader
    {
        public FullEntityLoader(ICachedEntities cache) : base(cache)
        { }

        protected override void LoadEntity<TEntity>(TEntity entity, IInternalSession session, DbDataReader reader)
        {
            foreach (var field in entity.GetTable().Fields)
            {
                SetValue(entity, session, field, reader.GetValue(field.FieldName), reader);
            }
        }
    }
}