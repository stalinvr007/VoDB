using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace VODB.Core.Loaders
{
    /// <summary>
    /// Maps a data reader into an Entity.
    /// </summary>
    interface IDbReaderMapper
    {
        /// <summary>
        /// Maps the specified reader.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> Map<TEntity>(IDataReader reader) where TEntity : new();
    }
}
