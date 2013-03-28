using System;
using System.Data;
using System.Data.Common;
using VODB.Core.Execution.Executers.DbResults;
using VODB.QueryCompiler;

namespace VODB
{
    /// <summary>
    /// Allows users to interact with a database using strongly typed Objects.
    /// </summary>
    public interface ISession : IDisposable
    {
        ITransaction BeginTransaction();

        /// <summary>
        /// Executes the T-SQL statements.
        /// </summary>
        /// <param name="SqlStatements">The SQL statements.</param>
        void ExecuteTSql(String SqlStatements);

        /// <summary>
        /// Gets all entities of the given type from this session.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        IQueryCompilerLevel1<TEntity> GetAll<TEntity>() where TEntity : class, new();

        /// <summary>
        /// Gets the entity by Id.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity that contains the key fields filled.</param>
        /// <returns></returns>
        TEntity GetById<TEntity>(TEntity entity) where TEntity : class, new();

        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        TEntity Insert<TEntity>(TEntity entity) where TEntity : class, new();

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns>True if deleted</returns>
        bool Delete<TEntity>(TEntity entity) where TEntity : class, new();

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        TEntity Update<TEntity>(TEntity entity) where TEntity : class, new();

        /// <summary>
        /// Gets the count of entities of the given type.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        int Count<TEntity>() where TEntity : class, new();

        /// <summary>
        /// Verifies if the entity exists in this session.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        bool Exists<TEntity>(TEntity entity) where TEntity : class, new();
    }

    internal interface IInternalSession : ISession
    {
        DbCommand CreateCommand();
        DbCommand RefreshCommand(DbCommand command);
        void Open();
        void Close();
    }
}