using System.Data.Common;

namespace VODB.DbLayer.Loaders
{
    /// <summary>
    /// Loads all the data to the entity from the DataReader.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    internal class FullEntityLoader<TModel> : EntityLoader<TModel>
        where TModel : Entity, new()
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
            if (entity == null) return;
            foreach (var field in entity.Table.Fields)
            {
                SetValue(entity, field, GetValue(reader, field.FieldName), reader);
            }

            entity.Session = _Session;
            entity.IsLoaded = true;
        }
    }
}