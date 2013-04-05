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
        
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IDbAndQueryResult<TEntity> : IEnumerable<TEntity>, IDbResult, IQueryCompilerLevel2<TEntity>
        where TEntity : class, new()
    {
        
    }

    public interface IDbOrderedDescResult<TEntity> : IEnumerable<TEntity>, IDbResult, IQueryCompilerStub<TEntity>
        where TEntity : class, new()
    {
    }

    public interface IDbOrderedResult<TEntity> : IEnumerable<TEntity>, IDbResult, IQueryCompilerLevel3<TEntity>
        where TEntity : class, new()
    {
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
        
    }
}