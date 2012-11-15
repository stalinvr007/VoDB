using System;
using System.Collections.Generic;
using System.Linq;
using VODB.DbLayer;
using VODB.DbLayer.DbCommands;
using VODB.DbLayer.DbExecuters;
using VODB.DbLayer.Loaders;

namespace VODB.Sessions
{
    public class PublicSessionBase : ISession
    {
        private ISession _InnerSession;

        public PublicSessionBase(ISession innerSession)
        {
            _InnerSession = innerSession;
        }

        public ITransaction BeginTransaction()
        {
            return _InnerSession.BeginTransaction();
        }

        public IDbQueryResult<TEntity> GetAll<TEntity>() where TEntity : DbEntity, new()
        {
            return _InnerSession.GetAll<TEntity>();
        }

        public System.Threading.Tasks.Task<IDbQueryResult<TEntity>> AsyncGetAll<TEntity>() where TEntity : DbEntity, new()
        {
            return _InnerSession.AsyncGetAll<TEntity>();
        }

        public TEntity GetById<TEntity>(TEntity entity) where TEntity : DbEntity, new()
        {
            return _InnerSession.GetById<TEntity>(entity);
        }

        public System.Threading.Tasks.Task<TEntity> AsyncGetById<TEntity>(TEntity entity) where TEntity : DbEntity, new()
        {
            return _InnerSession.AsyncGetById<TEntity>(entity);
        }

        public TEntity Insert<TEntity>(TEntity entity) where TEntity : DbEntity, new()
        {
            return _InnerSession.Insert(entity);
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : DbEntity, new()
        {
            _InnerSession.Delete(entity);
        }

        public TEntity Update<TEntity>(TEntity entity) where TEntity : DbEntity, new()
        {
            return _InnerSession.Update(entity);
        }

        public int Count<TEntity>() where TEntity : DbEntity, new()
        {
            return _InnerSession.Count<TEntity>();
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
