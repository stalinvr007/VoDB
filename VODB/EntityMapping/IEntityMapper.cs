using System.Data;
using VODB.Infrastructure;

namespace VODB.EntityMapping
{

    /// <summary>
    /// Maps an entity based on a DataReader.
    /// </summary>
    public interface IEntityMapper
    {

        /// <summary>
        /// Maps the specified entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="table">The table.</param>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        TEntity Map<TEntity>(TEntity entity, ITable table, IDataReader reader);

    }
}
