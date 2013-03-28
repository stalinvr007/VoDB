using VODB.Core;
using VODB.Core.Execution.Executers.DbResults;
using VODB.DbLayer;
using VODB.QueryCompiler;

namespace VODB
{
    public class Session : ISession
    {
        private ISession _InternalSession;

        public Session()
        {
            _InternalSession = Engine.Get<ISession>();
        }

        public Session(IDbConnectionCreator connectionCreator)
        {
            _InternalSession = Engine.Get<ISession>("creator", connectionCreator);
        }

        #region ISession Members

        public ITransaction BeginTransaction()
        {
            return _InternalSession.BeginTransaction();
        }

        public void ExecuteTSql(string SqlStatements)
        {
            _InternalSession.ExecuteTSql(SqlStatements);
        }

        public IQueryCompilerLevel1<TEntity> GetAll<TEntity>() where TEntity : class, new()
        {
            return _InternalSession.GetAll<TEntity>();
        }

        public TEntity GetById<TEntity>(TEntity entity) where TEntity : class, new()
        {
            return _InternalSession.GetById(entity);
        }

        public TEntity Insert<TEntity>(TEntity entity) where TEntity : class, new()
        {
            return _InternalSession.Insert(entity);
        }

        public bool Delete<TEntity>(TEntity entity) where TEntity : class, new()
        {
            return _InternalSession.Delete(entity);
        }

        public TEntity Update<TEntity>(TEntity entity) where TEntity : class, new()
        {
            return _InternalSession.Update(entity);
        }

        public int Count<TEntity>() where TEntity : class, new()
        {
            return _InternalSession.Count<TEntity>();
        }

        public bool Exists<TEntity>(TEntity entity) where TEntity : class, new()
        {
            return _InternalSession.Exists(entity);
        }

        public void Dispose()
        {
            if (_InternalSession == null) return;

            _InternalSession.Dispose();
            _InternalSession = null;
        }

        #endregion
    }
}