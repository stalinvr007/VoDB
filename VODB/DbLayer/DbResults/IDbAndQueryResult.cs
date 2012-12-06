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
    public interface IDbAndQueryResult<TEntity> : IEnumerable<TEntity>
    {
        /// <summary>
        /// Adds more conditions to the query.
        /// </summary>
        /// <param name="andCondition">The and condition.</param>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        IDbAndQueryResult<TEntity> And(string andCondition, params object[] args);

        /// <summary>
        /// Adds more conditions to the query.
        /// </summary>
        /// <param name="andCondition">The and condition.</param>
        /// <returns></returns>
        IDbAndQueryResult<TEntity> And(Expression<Func<TEntity, Boolean>> andCondition);

        /// <summary>
        /// Appends a Order by clause.
        /// </summary>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="orderByField">The order by field.</param>
        /// <returns></returns>
        IDbOrderedResult<TEntity> OrderBy<TField>(Expression<Func<TEntity, TField>> orderByField);
    }
}
