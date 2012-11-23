using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using VODB.DbLayer.DbExecuters;

namespace VODB
{
    /// <summary>
    /// Allows users to interact with a database using strongly typed Objects.
    /// </summary>
    public interface ISession : IDisposable
    {
        ITransaction BeginTransaction();

        /// <summary>
        /// Gets all entities of the given type from this session.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        IDbQueryResult<TEntity> GetAll<TEntity>() where TEntity : Entity, new();

        /// <summary>
        /// Gets all entities of the given type from this session. Asynchronously.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        Task<IDbQueryResult<TEntity>> AsyncGetAll<TEntity>() where TEntity : Entity, new();

        /// <summary>
        /// Gets the entity by Id.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity that contains the key fields filled.</param>
        /// <returns></returns>
        TEntity GetById<TEntity>(TEntity entity) where TEntity : Entity, new();

        /// <summary>
        /// Gets the entity by Id. Asynchronously.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity that contains the key fields filled.</param>
        /// <returns></returns>
        Task<TEntity> AsyncGetById<TEntity>(TEntity entity) where TEntity : Entity, new();

        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        TEntity Insert<TEntity>(TEntity entity) where TEntity : Entity, new();

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        void Delete<TEntity>(TEntity entity) where TEntity : Entity, new();

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        TEntity Update<TEntity>(TEntity entity) where TEntity : Entity, new();

        /// <summary>
        /// Gets the count of entities of the given type.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        int Count<TEntity>() where TEntity : Entity, new();

        /// <summary>
        /// Verifies if the entity exists in this session.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        bool Exists<TEntity>(TEntity entity) where TEntity : Entity, new();
    }

    internal interface IInternalSession
    {
        DbCommand CreateCommand();
        void Open();
        void Close();

    }
}