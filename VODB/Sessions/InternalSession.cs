using System;
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
    internal abstract class InternalSession : IInternalSession, ISession
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

        public abstract IDbQueryResult<TEntity> GetAll<TEntity>() where TEntity : DbEntity, new();

        public Task<IDbQueryResult<TEntity>> AsyncGetAll<TEntity>() where TEntity : DbEntity, new()
        {
            return _tasks.Add<IDbQueryResult<TEntity>>(
                new Task<IDbQueryResult<TEntity>>(new InternalEagerSession(_creator).GetAll<TEntity>).RunAsync()
            );
        }

        public abstract TEntity GetById<TEntity>(TEntity entity) where TEntity : DbEntity, new();

        public Task<TEntity> AsyncGetById<TEntity>(TEntity entity) where TEntity : DbEntity, new()
        {
            return _tasks.Add<TEntity>(
                new Task<TEntity>(() => new InternalEagerSession(_creator).GetById(entity)).RunAsync()
            );
        }

        public TEntity Insert<TEntity>(TEntity entity) where TEntity : DbEntity, new()
        {
            var idField = entity.Table.KeyFields.FirstOrDefault(f => f.IsIdentity);

            var id = Run(() =>
            {
                new DbCommandNonQueryExecuter(
                    new DbEntityInsertCommandFactory<TEntity>(this, entity)
                    ).Execute();

                if (idField == null)
                {
                    return null;
                }

                return new DbQueryScalarExecuter<Object>(
                    new DbCommandBypass(this, "Select @@IDENTITY").Make()).Execute();
            });

            if (idField != null)
            {
                entity.SetValue(idField,
                    Convert.ChangeType(id, idField.FieldType),
                    field => Convert.ChangeType(id, idField.FieldType));
            }
            
            return entity;
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : DbEntity, new()
        {
            Run(() =>
                new DbCommandNonQueryExecuter(
                    new DbEntityDeleteCommandFactory<TEntity>(this, entity)
                ).Execute()
            );
        }

        public TEntity Update<TEntity>(TEntity entity) where TEntity : DbEntity, new()
        {
            Run(() =>
                new DbCommandNonQueryExecuter(
                    new DbEntityUpdateCommandFactory<TEntity>(this, entity)
                ).Execute()
            );
            return entity;
        }

        public int Count<TEntity>() where TEntity : DbEntity, new()
        {
            return Run(() =>
                new DbQueryScalarExecuter<int>(
                    new DbEntityCountCommandFactory<TEntity>(this).Make()
                ).Execute()
            );
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

        protected TResult Run<TResult>(Func<TResult> action)
        {
            Open();
            try
            {
                return action();
            }
            catch (Exception)
            {
                if (InTransaction)
                {
                    _transaction.RollBack();
                }

                throw;
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
            Close();
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