using System.Data.Common;
using VODB.Core;

namespace VODB.DbLayer.Loaders
{
    /// <summary>
    /// Loads the key fields from the datareader.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    internal class EntityKeyLoader<TModel> : EntityLoader<TModel>
        where TModel : new()
    {

        /// <summary>
        /// Loads the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public override void Load(TModel entity, DbDataReader reader)
        {
            if (entity == null) return;
            var table = Engine.GetTable(entity.GetType());

            foreach (var field in table.KeyFields)
            {
                SetValue(entity, field, GetValue(reader, field.FieldName), reader);
            }
        }
    }
}