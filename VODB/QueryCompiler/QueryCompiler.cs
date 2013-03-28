using System.Collections.Generic;
using System;
using VODB.Core.Execution.Executers.DbResults;
using System.Linq.Expressions;
using System.Collections;
using VODB.ExpressionsToSql;
using VODB.EntityTranslation;

namespace VODB.QueryCompiler
{

    /// <summary>
    /// This interface is used to represent the operations available at the first level.
    /// 
    /// Available features
    /// Where, OrderBy
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IQueryCompilerLevel1<TEntity> : IEnumerable<TEntity>
    {
        /// <summary>
        /// Start of a where clause that match a given value.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns></returns>
        IQueryCompilerLevel2<TEntity> Where(Expression<Func<TEntity, Boolean>> condition);
    }

    /// <summary>
    /// This interface is used to represent the operations available at the second level.
    /// 
    /// Available features
    /// And, Or, OrderBy
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IQueryCompilerLevel2<TEntity> : IEnumerable<TEntity>
    {

    }

    class QueryCompiler<TEntity> : IQuery<TEntity>, IQueryCompilerLevel1<TEntity>, IQueryCompilerLevel2<TEntity>
    {
        /// <summary>
        /// Holds the conditions and enables compilation.
        /// </summary>
        private IQueryConditionComposite _Query = new QueryCondition();
        private IEntityTranslator _Translator;

        public QueryCompiler(IEntityTranslator translator, Func<IQueryCompilerLevel1<TEntity>, IEnumerable<TEntity>> func)
        {
            _Translator = translator;
            func(this);
        }

        #region IQueryCompilerLevel1<TEntity> Implementation

        public IQueryCompilerLevel2<TEntity> Where(Expression<Func<TEntity, bool>> condition)
        {
            _Query.Add(new QueryCondition<TEntity>(_Translator, condition));
            return this;
        }

        #endregion

        #region IEnumerable<TEntity> Implementation

        public IEnumerator<TEntity> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion


        #region IQuery<TEntity> Implementation

        public IEnumerable<TEntity> Execute(ISession session)
        {
            throw new System.NotImplementedException();
        }

        public string Compile(ref int level)
        {
            return _Translator.Translate(typeof(TEntity)).SqlSelect + _Query.Compile(ref level);
        }

        public IEnumerable<IQueryParameter> Parameters
        {
            get
            {
                return _Query.Parameters;
            }
        }

        #endregion

    }
}
