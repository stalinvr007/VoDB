using VODB.DbLayer.DbResults;

namespace VODB.Sessions
{
    public class PublicSessionBase : ISession
    {
        private ISession _InnerSession;

        protected PublicSessionBase(ISession innerSession)
        {
            _InnerSession = innerSession;
        }

        public ITransaction BeginTransaction()
        {
            return _InnerSession.BeginTransaction();
        }

        public void ExecuteTSql(string SqlStatements)
        {
            _InnerSession.ExecuteTSql(SqlStatements);
        }

        public IDbQueryResult<TEntity> GetAll<TEntity>() where TEntity : new()
        {
            return _InnerSession.GetAll<TEntity>();
        }

        public System.Threading.Tasks.Task<IDbQueryResult<TEntity>> AsyncGetAll<TEntity>() where TEntity : new()
        {
            return _InnerSession.AsyncGetAll<TEntity>();
        }

        public TEntity GetById<TEntity>(TEntity entity) where TEntity : new()
        {
            return _InnerSession.GetById(entity);
        }

        public System.Threading.Tasks.Task<TEntity> AsyncGetById<TEntity>(TEntity entity) where TEntity : new()
        {
            return _InnerSession.AsyncGetById(entity);
        }

        public TEntity Insert<TEntity>(TEntity entity) where TEntity : new()
        {
            return _InnerSession.Insert(entity);
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : new()
        {
            _InnerSession.Delete(entity);
        }

        public TEntity Update<TEntity>(TEntity entity) where TEntity : new()
        {
            return _InnerSession.Update(entity);
        }

        public int Count<TEntity>() where TEntity : new()
        {
            return _InnerSession.Count<TEntity>();
        }

        public bool Exists<TEntity>(TEntity entity) where TEntity : new()
        {
            return _InnerSession.Exists(entity);
        }

        public void Dispose()
        {
            if (_InnerSession != null)
            {
                _InnerSession.Dispose();
                _InnerSession = null;
            }
        }

        
    }
}
