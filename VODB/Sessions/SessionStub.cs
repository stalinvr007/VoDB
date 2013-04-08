using System;
using VODB.QueryCompiler;

namespace VODB.Sessions
{
    class SessionStub : ISession
    {
        public ITransaction BeginTransaction()
        {
            throw new NotImplementedException();
        }

        public void ExecuteTSql(string SqlStatements)
        {
            throw new NotImplementedException();
        }

        public IQueryCompilerLevel1<TEntity> GetAll<TEntity>() where TEntity : class, new()
        {
            throw new NotImplementedException();
        }

        public System.Collections.Generic.IEnumerable<TEntity> ExecuteQuery<TEntity>(IQuery<TEntity> query, Object[] args) where TEntity : class, new()
        {
            throw new NotImplementedException();
        }

        public TEntity GetById<TEntity>(TEntity entity) where TEntity : class, new()
        {
            throw new NotImplementedException();
        }

        public TEntity Insert<TEntity>(TEntity entity) where TEntity : class, new()
        {
            throw new NotImplementedException();
        }

        public bool Delete<TEntity>(TEntity entity) where TEntity : class, new()
        {
            throw new NotImplementedException();
        }

        public TEntity Update<TEntity>(TEntity entity) where TEntity : class, new()
        {
            throw new NotImplementedException();
        }

        public int Count<TEntity>() where TEntity : class, new()
        {
            throw new NotImplementedException();
        }

        public bool Exists<TEntity>(TEntity entity) where TEntity : class, new()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public string DataBaseName
        {
            get { throw new NotImplementedException(); }
        }
    }
}
