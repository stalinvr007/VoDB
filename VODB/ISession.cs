using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace VODB
{
    /// <summary>
    /// Allows users to interact with a database using strongly typed Objects.
    /// </summary>
    public interface ISession
    {
        ITransaction BeginTransaction();

        /// <summary>
        /// Gets all entities of the given type from this session.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        IEnumerable<TEntity> GetAll<TEntity>() where TEntity : DbEntity, new();

        /// <summary>
        /// Gets all entities of the given type from this session. Asynchronously.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> AsyncGetAll<TEntity>() where TEntity : DbEntity, new();

        /// <summary>
        /// Gets the entity by Id.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity that contains the key fields filled.</param>
        /// <returns></returns>
        TEntity GetById<TEntity>(TEntity entity) where TEntity : DbEntity, new();

        /// <summary>
        /// Gets the entity by Id. Asynchronously.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity that contains the key fields filled.</param>
        /// <returns></returns>
        Task<TEntity> AsyncGetById<TEntity>(TEntity entity) where TEntity : DbEntity, new();

        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        TEntity Insert<TEntity>(TEntity entity) where TEntity : DbEntity, new();
    }

    internal interface IInternalSession
    {
        DbCommand CreateCommand();

        void Open();

        void Close();
    }
}