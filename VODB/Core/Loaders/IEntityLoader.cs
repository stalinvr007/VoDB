using System.Data.Common;

namespace VODB.Core.Loaders
{
    internal interface IEntityLoader
    {
        /// <summary>
        /// Loads the specified entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="session"></param>
        /// <param name="reader">The reader.</param>
        void Load<TEntity>(TEntity entity, IInternalSession session, DbDataReader reader);
    }
}