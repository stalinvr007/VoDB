using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VODB.Core.Infrastructure;

namespace VODB.Core.Loaders
{
    /// <summary>
    /// Maps a IDictionary into an Entity
    /// </summary>
    internal interface IDictionaryMapper
    {
        /// <summary>
        /// Maps the specified data.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="data">The data.</param>
        /// <param name="entityTable">The entity table.</param>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        Task<TEntity> Map<TEntity>(IDictionary<string, object> data, Table entityTable, TEntity entity);
    }

    internal class DictionaryMapper : IDictionaryMapper
    {
        #region Implementation of IDictionaryMapper

        public Task<TEntity> Map<TEntity>(IDictionary<string, object> data, Table entityTable, TEntity entity)
        {
            return Task<TEntity>.Factory.StartNew(() => {
                // For performance test only...
                Thread.SpinWait(5000);
                return entity;
            });
        }

        #endregion
    }
}