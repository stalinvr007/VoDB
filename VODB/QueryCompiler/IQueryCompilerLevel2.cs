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
    /// This interface is used to represent the operations available at the second level.
    /// 
    /// Available features
    /// And, Or, OrderBy
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IQueryCompilerLevel2<TEntity> : IQuery<TEntity>, IQueryCompilerCommun<TEntity>
        where TEntity : class, new()
    {
        /// <summary>
        /// Appends the Order By clause.
        /// </summary>
        /// <param name="orderByField">The order by field.</param>
        /// <returns></returns>
        IQueryCompilerLevel3<TEntity> OrderBy<TField>(Expression<Func<TEntity, TField>> expression);

        /// <summary>
        /// Appends another condition to the Query.
        /// </summary>
        /// <param name="andCondition">The expression.</param>
        /// <returns></returns>
        IQueryCompilerLevel2<TEntity> And(Expression<Func<TEntity, Boolean>> expression);

        /// <summary>
        /// Appends another condition to the Query.
        /// </summary>
        /// <param name="mask">The mask.</param>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        IQueryCompilerLevel2<TEntity> And(String mask, params object[] args);

        /// <summary>
        /// Appends another condition to the Query.
        /// </summary>
        /// <param name="andCondition">The expression.</param>
        /// <returns></returns>
        IQueryCompilerLevel4<TEntity> And<TField>(Expression<Func<TEntity, TField>> expression);

        /// <summary>
        /// Appends another condition to the Query.
        /// </summary>
        /// <param name="andCondition">The expression.</param>
        /// <returns></returns>
        IQueryCompilerLevel2<TEntity> Or(Expression<Func<TEntity, Boolean>> expression);

        /// <summary>
        /// Appends another condition to the Query.
        /// </summary>
        /// <param name="andCondition">The expression.</param>
        /// <returns></returns>
        IQueryCompilerLevel4<TEntity> Or<TField>(Expression<Func<TEntity, TField>> expression);

    }
}
