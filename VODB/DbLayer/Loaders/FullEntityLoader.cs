using System.Data.Common;

namespace VODB.DbLayer.Loaders
{
    /// <summary>
    /// Loads all the data to the entity from the DataReader.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    internal class FullEntityLoader<TModel> : EntityLoader<TModel>
        where TModel : new()
    {

        private readonly ISession _Session;
        public FullEntityLoader(ISession session = null)
        {
            _Session = session;
        }

        /// <summary>
        /// Loads the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public override void Load(TModel entity, DbDataReader reader)
        {
            var inEntity = entity as Entity;
            if (inEntity == null) return;
            foreach (var field in inEntity.Table.Fields)
            {
                SetValue(entity, field, GetValue(reader, field.FieldName), reader);
            }

            inEntity.Session = _Session;
            inEntity.IsLoaded = true;
        }
    }
}