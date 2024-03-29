using System.Data;
using VODB.Infrastructure;

namespace VODB.EntityMapping
{
    /// <summary>
    /// Maps an entity as long as the field order is the same as the reader retrieved columns.
    /// </summary>
    class OrderedEntityMapper : IEntityMapper
    {

        public TEntity Map<TEntity>(TEntity entity, ITable table, IDataReader reader)
        {
            int i = 0;
            foreach (var field in table.Fields)
            {
                field.SetFieldFinalValue(entity, reader.GetValue(i++));
            }

            return entity;
        }
    }
}
