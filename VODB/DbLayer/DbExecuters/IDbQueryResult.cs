using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq.Expressions;
using VODB.DbLayer.DbCommands;
using VODB.Exceptions;

namespace VODB.DbLayer.DbExecuters
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
        /// Wheres the specified where condition.
        /// </summary>
        /// <param name="whereCondition">The where condition.</param>
        /// <returns></returns>
        IDbAndQueryResult<TEntity> Where(Expression<Func<TEntity, Boolean>> whereCondition);
        

    }

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
    }


}
