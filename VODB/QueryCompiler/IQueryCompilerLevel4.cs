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
    /// This interface is used to represent the operations available at the forth level.
    /// 
    /// Available features
    /// Like, In, Between
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IQueryCompilerLevel4<TEntity>
        where TEntity : class, new()
    {
        /// <summary>
        /// Filters the field using the Like condition.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        IQueryCompilerLevel2<TEntity> Like(String value, WildCard token = WildCard.Both);

        /// <summary>
        /// filters the field withing the specified in the collection.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <returns></returns>
        IQueryCompilerLevel2<TEntity> In(IEnumerable<Object> collection);

        /// <summary>
        /// Filters the field using the between condition.
        /// </summary>
        /// <param name="firstValue">The first value.</param>
        /// <param name="secondValue">The second value.</param>
        /// <returns></returns>
        IQueryCompilerLevel2<TEntity> Between(Object firstValue, Object secondValue);
    }
}
