﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using VODB.ExpressionsToSql;
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
        /// Executes the query.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        /// <param name="args"></param>
        IEnumerable<TEntity> ExecuteQuery<TEntity>(IQuery<TEntity> query, params Object[] args) where TEntity : class, new();

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

        /// <summary>
        /// Gets the name of the data base.
        /// </summary>
        /// <value>
        /// The name of the data base.
        /// </value>
        String DataBaseName { get; }
    }

    internal interface IInternalSession : ISession
    {
        DbCommand CreateCommand();
        DbCommand RefreshCommand(DbCommand command);
        void Open();
        void Close();

        IEnumerable<TEntity> InternalExecuteQuery<TEntity>(IQuery<TEntity> query, params Object[] args) where TEntity : class, new();

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="args">The args.</param>
        /// <returns>
        /// The number of records afected by the command.
        /// </returns>
        int ExecuteNonQuery(String command, IEnumerable<IQueryParameter> args);

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="args">The args.</param>
        /// <returns>
        /// A DataReader with the result
        /// </returns>
        IDataReader ExecuteReader(String command, IEnumerable<IQueryParameter> args);

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="args">The args.</param>
        /// <returns>
        /// A simgle value.
        /// </returns>
        object ExecuteScalar(String command, IEnumerable<IQueryParameter> args);
    }
}