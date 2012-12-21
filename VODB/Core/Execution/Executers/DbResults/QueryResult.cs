using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using VODB.Core.Execution.Statements;
using VODB.Core.Infrastructure;
using VODB.Core.Loaders;
using VODB.Core.Loaders.Factories;
using VODB.ExpressionParser;

namespace VODB.Core.Execution.Executers.DbResults
{

    interface IQueryResultGetter
    {
        IDbQueryResult<TEntity> GetQueryResult<TEntity>(IInternalSession session, IEntityLoader loader, IEntityFactory entityFactory)
            where TEntity : class, new();

    }

    class QueryResultGetter : IQueryResultGetter
    {
        public IDbQueryResult<TEntity> GetQueryResult<TEntity>(IInternalSession session, IEntityLoader loader, IEntityFactory entityFactory)
            where TEntity : class, new()
        {
            return new QueryResult<TEntity>(session, loader, entityFactory);
        }
    }

    class QueryResult<TEntity> : IDbQueryResult<TEntity>
        where TEntity : class, new()
    {
        private readonly IInternalSession _Session;
        private readonly IEntityLoader _Loader;
        private readonly Table _Table;
        private readonly IEntityFactory _EntityFactory;

        public QueryResult(IInternalSession session, IEntityLoader loader, IEntityFactory entityFactory)
        {
            _EntityFactory = entityFactory;
            _Loader = loader;
            _Session = session;
            _Table = Engine.GetTable<TEntity>();
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
            var cmd = _Session.CreateCommand();
            _Session.Open();
            cmd.CommandText = WhereCondition;

            var reader = cmd.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    TEntity newTEntity = _EntityFactory.Make<TEntity>(_Session as ISession);
                    _Loader.Load(newTEntity, _Session as ISession, reader);
                    yield return newTEntity;
                }
            }
            finally
            {
                reader.Close();
            }
            
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
            get { return _Table.TableName; }
        }

        public string WhereCondition
        {
            get { return _Table.CommandsHolder.Select; }
        }
    }

}
