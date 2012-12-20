using System.Data.Common;

namespace VODB.Core.Loaders
{
    /// <summary>
    /// Loads all the data to the entity from the DataReader.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    internal class FullEntityLoader: EntityLoader
    {

        /// <summary>
        /// Loads the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public override void Load<TEntity>(TEntity entity, DbDataReader reader)
        {

            if (entity == null) return;
            foreach (var field in entity.GetTable().Fields)
            {
                SetValue(entity, field, GetValue(reader, field.FieldName), reader);
            }

        }
    }
}