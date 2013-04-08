using System;
using VODB.QueryCompiler;
using VODB.Extensions;

namespace VODB.Sessions
{
    public class SessionBase : ISession
    {
        private ISession _InternalSession;

        protected SessionBase(ISession internalSession)
        {
            _InternalSession = internalSession;
        }

        #region ISession Members

        public ITransaction BeginTransaction()
        {
            return _InternalSession.BeginTransaction();
        }

        public void ExecuteTSql(string SqlStatements)
        {
            _InternalSession.CaptureExceptions(session => session.ExecuteTSql(SqlStatements));
        }

        public IQueryCompilerLevel1<TEntity> GetAll<TEntity>() where TEntity : class, new()
        {
            return _InternalSession.CaptureExceptions(session => session.GetAll<TEntity>());
        }

        public System.Collections.Generic.IEnumerable<TEntity> ExecuteQuery<TEntity>(IQuery<TEntity> query, params Object[] args) where TEntity : class, new()
        {
            return _InternalSession.CaptureExceptions(session => session.ExecuteQuery<TEntity>(query, args));
        }

        public TEntity GetById<TEntity>(TEntity entity) where TEntity : class, new()
        {
            return _InternalSession.CaptureExceptions(session => session.GetById(entity));
        }

        public TEntity Insert<TEntity>(TEntity entity) where TEntity : class, new()
        {
            return _InternalSession.CaptureExceptions(session => session.Insert(entity));
        }

        public bool Delete<TEntity>(TEntity entity) where TEntity : class, new()
        {
            return _InternalSession.CaptureExceptions(session => session.Delete(entity));
        }

        public TEntity Update<TEntity>(TEntity entity) where TEntity : class, new()
        {
            return _InternalSession.CaptureExceptions(session => session.Update(entity));
        }

        public int Count<TEntity>() where TEntity : class, new()
        {
            return _InternalSession.CaptureExceptions(session => session.Count<TEntity>());
        }

        public bool Exists<TEntity>(TEntity entity) where TEntity : class, new()
        {
            return _InternalSession.CaptureExceptions(session => session.Exists(entity));
        }

        public void Dispose()
        {
            if (_InternalSession == null) return;

            _InternalSession.Dispose();
            _InternalSession = null;
        }

        public string DataBaseName
        {
            get { return _InternalSession.DataBaseName; }
        }

        #endregion

    }
}