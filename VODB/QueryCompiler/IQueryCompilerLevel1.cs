using System.Collections.Generic;
using System;
using VODB.Core.Execution.Executers.DbResults;
using System.Linq.Expressions;
using System.Collections;
using VODB.ExpressionsToSql;
using VODB.EntityTranslation;

namespace VODB.QueryCompiler
{

    public interface IQueryCompilerCommun<TEntity> : IEnumerable<TEntity>, IQueryCondition
    {
        /// <summary>
        /// Makes this a count query.
        /// </summary>
        /// <returns></returns>
        int Count();
    }

    /// <summary>
    /// This interface is used to represent the operations available at the first level.
    /// 
    /// Available features
    /// Where, OrderBy
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IQueryCompilerLevel1<TEntity> : IQuery<TEntity>, IQueryCompilerCommun<TEntity>
        where TEntity : class, new()
    {
        /// <summary>
        /// Start of a where clause that match a given value.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns></returns>
        IQueryCompilerLevel2<TEntity> Where(Expression<Func<TEntity, Boolean>> expression);

        /// <summary>
        /// Start of a where clause.
        /// </summary>
        /// <param name="mask">The mask.</param>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        IQueryCompilerLevel2<TEntity> Where(String mask, params object[] args);

        /// <summary>
        /// Start of a where clause.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns></returns>
        IQueryCompilerLevel4<TEntity> Where<TField>(Expression<Func<TEntity, TField>> expression);

        /// <summary>
        /// Appends the Order By clause.
        /// </summary>
        /// <param name="orderByField">The order by field.</param>
        /// <returns></returns>
        IQueryCompilerLevel3<TEntity> OrderBy<TField>(Expression<Func<TEntity, TField>> expression);

    }
}
