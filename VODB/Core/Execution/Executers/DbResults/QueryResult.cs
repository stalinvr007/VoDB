using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using VODB.Core.Execution.Statements;
using VODB.Core.Loaders;
using VODB.ExpressionParser;

namespace VODB.Core.Execution.Executers.DbResults
{

    interface IQueryResultGetter
    {
        IDbQueryResult<TEntity> GetQueryResult<TEntity>(IInternalSession session, IEntityLoader loader);
    }

    class QueryResultGetter : IQueryResultGetter
    {
        public IDbQueryResult<TEntity> GetQueryResult<TEntity>(IInternalSession session, IEntityLoader loader)
        {
            return new QueryResult<TEntity>(session, loader);
        }
    }

    class QueryResult<TEntity> : IDbQueryResult<TEntity>
    {
        private readonly IInternalSession _Session;
        private readonly IEntityLoader _Loader;
        
        public QueryResult(IInternalSession session, IEntityLoader loader)
        {
            _Loader = loader;
            _Session = session; 
        }

        public IDbAndQueryResult<TEntity> Where(string whereCondition, params object[] args)
        {
            throw new NotImplementedException();
        }

        public IDbAndQueryResult<TEntity> Where(Expression<Func<TEntity, bool>> whereCondition)
        {
            throw new NotImplementedException();
        }

        public IDbFieldFilterResult<TEntity> Where<TField>(Expression<Func<TEntity, TField>> field)
        {
            throw new NotImplementedException();
        }

        public IDbOrderedResult<TEntity> OrderBy<TField>(Expression<Func<TEntity, TField>> orderByField)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<TEntity> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerable<KeyValuePair<Key, object>> Parameters
        {
            get { throw new NotImplementedException(); }
        }

        public string TableName
        {
            get { throw new NotImplementedException(); }
        }

        public string WhereCondition
        {
            get { throw new NotImplementedException(); }
        }
    }

}
