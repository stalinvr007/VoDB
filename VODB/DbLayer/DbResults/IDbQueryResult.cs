using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq.Expressions;
using VODB.DbLayer.DbCommands;
using VODB.Exceptions;

namespace VODB.DbLayer.DbResults
{

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IDbQueryResult<TEntity> : IEnumerable<TEntity>
    {

        /// <summary>
        /// Appends a where condition.
        /// </summary>
        /// <param name="whereCondition">The where condition.</param>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        IDbAndQueryResult<TEntity> Where(string whereCondition, params object[] args);

        /// <summary>
        /// Appends the specified where condition.
        /// </summary>
        /// <param name="whereCondition">The where condition.</param>
        /// <returns></returns>
        IDbAndQueryResult<TEntity> Where(Expression<Func<TEntity, Boolean>> whereCondition);

        /// <summary>
        /// Appends a Order by clause.
        /// </summary>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="orderByField">The order by field.</param>
        /// <returns></returns>
        IDbOrderedResult<TEntity> OrderBy<TField>(Expression<Func<TEntity, TField>> orderByField);
    }


}
