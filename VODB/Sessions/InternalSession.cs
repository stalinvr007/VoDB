﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using VODB.DbLayer;
using VODB.DbLayer.DbCommands;
using VODB.DbLayer.DbExecuters;
using VODB.Extensions;

namespace VODB.Sessions
{
    /// <summary>
    /// Represents a connection Session.
    /// </summary>
    internal abstract class InternalSession : IInternalSession, ISession, IDisposable
    {
        private IDbConnectionCreator _creator;
        private DbConnection _connection;
        private Transaction _transaction;
        private TasksCollection _tasks;

        /// <summary>
        /// Initializes a new instance of the <see cref="InternalSession" /> class.
        /// </summary>
        /// <param name="creator">The creator.</param>
        internal InternalSession(IDbConnectionCreator creator = null)
        {
            _creator = creator ?? new SqlConnectionCreator();
            _tasks = new TasksCollection();
        }

        private bool InTransaction
        {
            get { return _transaction != null; }
        }

        #region ISession Members

        public ITransaction BeginTransaction()
        {
            lock (this)
            {
                CreateConnection();

                if (_transaction != null)
                {
                    _transaction.BeginNestedTransaction();
                }
                else
                {
                    _transaction = new Transaction(_connection.BeginTransaction());
                }

                return _transaction;
            }
        }

        public abstract IEnumerable<TEntity> GetAll<TEntity>() where TEntity : DbEntity, new();

        public Task<IEnumerable<TEntity>> AsyncGetAll<TEntity>() where TEntity : DbEntity, new()
        {
            return _tasks.Add<IEnumerable<TEntity>>(
                new Task<IEnumerable<TEntity>>(new EagerSession(_creator).GetAll<TEntity>).RunAsync()
            );
        }

        public abstract TEntity GetById<TEntity>(TEntity entity) where TEntity : DbEntity, new();

        public Task<TEntity> AsyncGetById<TEntity>(TEntity entity) where TEntity : DbEntity, new()
        {
            return _tasks.Add<TEntity>(
                new Task<TEntity>(() => new EagerSession(_creator).GetById(entity)).RunAsync()
            );
        }

        public TEntity Insert<TEntity>(TEntity entity) where TEntity : DbEntity, new()
        {
            var id = RunAndClose(() =>
            {
                new DbCommandNonQueryExecuter(
                    new DbEntityInsertCommandFactory<TEntity>(this, entity)
                    ).Execute();

                return new DbQueryExecuterCommandEager(
                    new DbCommandBypass(this, "Select @@IDENTITY").Make(), entity.Table).Execute().FirstOrDefault();
            });

            var idField = entity.Table.KeyFields.FirstOrDefault(f => f.IsIdentity);
            if (idField != null)
            {
                entity.SetValue(idField, id, field => id);
            }
            return entity;
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : DbEntity, new()
        {
            RunAndClose(() =>
                new DbCommandNonQueryExecuter(
                    new DbEntityDeleteCommandFactory<TEntity>(this, entity)
                ).Execute()
            );
        }

        public TEntity Update<TEntity>(TEntity entity) where TEntity : DbEntity, new()
        {
            RunAndClose(() =>
                new DbCommandNonQueryExecuter(
                    new DbEntityUpdateCommandFactory<TEntity>(this, entity)
                ).Execute()
            );
            return entity;
        }

        #endregion

        #region IInternalSession Members

        public DbCommand CreateCommand()
        {
            lock (this)
            {
                CreateConnection();

                return InTransaction
                           ? _transaction.CreateCommand()
                           : _connection.CreateCommand();
            }
        }


        public void Open()
        {
            lock (this)
            {
                CreateConnection();

                if (_connection.State == ConnectionState.Open)
                {
                    return;
                }
                _connection.Open();
            }

        }

        public void Close()
        {
            lock (this)
            {
                if (_connection == null || _connection.State == ConnectionState.Closed ||
                    (InTransaction && !_transaction.Ended) || _tasks.Count > 0)
                {
                    return;
                }
                _connection.Close();
            }
        }

        #endregion

        protected TResult RunAndClose<TResult>(Func<TResult> action)
        {
            Open();
            try
            {
                return action();
            }
            finally
            {
                Close();
            }
        }

        private void CreateConnection()
        {
            if (_connection == null)
            {
                _connection = _creator.Create();
            }
        }

        public void Dispose()
        {

            if (_transaction != null)
            {
                _transaction.Dispose();
                _transaction = null;
            }

            if (_connection != null)
            {
                _connection.Dispose();
                _connection = null;
            }

            _creator = null;
            _tasks = null;
        }
    }
}