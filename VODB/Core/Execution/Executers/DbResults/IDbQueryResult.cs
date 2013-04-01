using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using VODB.ExpressionParser;
using VODB.QueryCompiler;

namespace VODB.Core.Execution.Executers.DbResults
{
    public interface IDbResult
    {
        IEnumerable<KeyValuePair<Key, Object>> Parameters { get; }

        String TableName { get; }

        String WhereCondition { get; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IDbQueryResult<TEntity> : IEnumerable<TEntity>, IDbResult, IQueryCompilerLevel1<TEntity>
        where TEntity : class, new()
    {
        /// <summary>
        /// Appends a where condition.
        /// </summary>
        /// <param name="whereCondition">The where condition.</param>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        new IDbAndQueryResult<TEntity> Where(string whereCondition, params object[] args);

        /// <summary>
        /// Appends the specified where condition.
        /// </summary>
        /// <param name="whereCondition">The where condition.</param>
        /// <returns></returns>
        new IDbAndQueryResult<TEntity> Where(Expression<Func<TEntity, Boolean>> whereCondition);

        /// <summary>
        /// Starts a field filter.
        /// </summary>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="field">The field.</param>
        /// <returns></returns>
        IDbFieldFilterResult<TEntity> Where<TField>(Expression<Func<TEntity, TField>> field);

        /// <summary>
        /// Appends a Order by clause.
        /// </summary>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="orderByField">The order by field.</param>
        /// <returns></returns>
        IDbOrderedResult<TEntity> OrderBy<TField>(Expression<Func<TEntity, TField>> orderByField);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IDbAndQueryResult<TEntity> : IEnumerable<TEntity>, IDbResult, IQueryCompilerLevel2<TEntity>
        where TEntity : class, new()
    {
        /// <summary>
        /// Adds more conditions to the query.
        /// </summary>
        /// <param name="andCondition">The and condition.</param>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        new IDbAndQueryResult<TEntity> And(string andCondition, params object[] args);

        /// <summary>
        /// Adds more conditions to the query.
        /// </summary>
        /// <param name="andCondition">The and condition.</param>
        /// <returns></returns>
        new IDbAndQueryResult<TEntity> And(Expression<Func<TEntity, Boolean>> andCondition);

        /// <summary>
        /// Starts a field filter.
        /// </summary>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="field">The field.</param>
        /// <returns></returns>
        IDbFieldFilterResult<TEntity> Or<TField>(Expression<Func<TEntity, TField>> field);

        /// <summary>
        /// Adds more conditions to the query.
        /// </summary>
        /// <param name="andCondition">The and condition.</param>
        /// <returns></returns>
        new IDbAndQueryResult<TEntity> Or(Expression<Func<TEntity, Boolean>> orCondition);

        /// <summary>
        /// Starts a field filter.
        /// </summary>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="field">The field.</param>
        /// <returns></returns>
        IDbFieldFilterResult<TEntity> And<TField>(Expression<Func<TEntity, TField>> field);

        /// <summary>
        /// Appends a Order by clause.
        /// </summary>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="orderByField">The order by field.</param>
        /// <returns></returns>
        IDbOrderedResult<TEntity> OrderBy<TField>(Expression<Func<TEntity, TField>> orderByField);
    }

    public interface IDbOrderedDescResult<TEntity> : IEnumerable<TEntity>, IDbResult, IQueryCompilerStub<TEntity>
        where TEntity : class, new()
    {
    }

    public interface IDbOrderedResult<TEntity> : IEnumerable<TEntity>, IDbResult, IQueryCompilerLevel3<TEntity>
        where TEntity : class, new()
    {
        new IDbOrderedDescResult<TEntity> Descending();
    }


    public enum WildCard
    {
        Left,
        Right,
        Both,
        None
    }

    public interface IDbFieldFilterResult<TEntity> : IDbResult, IQueryCompilerLevel4<TEntity>
        where TEntity : class, new()
    {
        /// <summary>
        /// Likes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        new IDbAndQueryResult<TEntity> Like(String value, WildCard token = WildCard.Both);

        /// <summary>
        /// filters the field withing the specified in the collection.
        /// </summary>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <returns></returns>
        IDbAndQueryResult<TEntity> In<TField>(IEnumerable<TField> collection);

        /// <summary>
        /// Filters the field using the between condition.
        /// </summary>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="firstValue">The first value.</param>
        /// <param name="secondValue">The second value.</param>
        /// <returns></returns>
        IDbAndQueryResult<TEntity> Between<TField>(TField firstValue, TField secondValue);
    }
}