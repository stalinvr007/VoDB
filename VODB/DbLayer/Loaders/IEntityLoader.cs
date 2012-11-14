using System.Data.Common;

namespace VODB.DbLayer.Loaders
{
    internal interface IEntityLoader<in TModel> where TModel : DbEntity, new()
    {
        /// <summary>
        /// Loads the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        void Load(TModel entity, DbDataReader reader);
    }
}