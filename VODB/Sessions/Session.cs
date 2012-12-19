using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VODB.DbLayer.DbResults;

namespace VODB.Sessions
{
    class Session : ISession
    {

        public ITransaction BeginTransaction()
        {
            throw new NotImplementedException();
        }

        public void ExecuteTSql(string SqlStatements)
        {
            throw new NotImplementedException();
        }

        public IDbQueryResult<TEntity> GetAll<TEntity>() where TEntity : new()
        {
            throw new NotImplementedException();
        }

        public Task<IDbQueryResult<TEntity>> AsyncGetAll<TEntity>() where TEntity : new()
        {
            throw new NotImplementedException();
        }

        public TEntity GetById<TEntity>(TEntity entity) where TEntity : new()
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> AsyncGetById<TEntity>(TEntity entity) where TEntity : new()
        {
            throw new NotImplementedException();
        }

        public TEntity Insert<TEntity>(TEntity entity) where TEntity : new()
        {
            throw new NotImplementedException();
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : new()
        {
            throw new NotImplementedException();
        }

        public TEntity Update<TEntity>(TEntity entity) where TEntity : new()
        {
            throw new NotImplementedException();
        }

        public int Count<TEntity>() where TEntity : new()
        {
            throw new NotImplementedException();
        }

        public bool Exists<TEntity>(TEntity entity) where TEntity : new()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
