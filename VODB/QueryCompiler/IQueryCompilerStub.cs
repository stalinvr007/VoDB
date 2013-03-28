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
    /// This interface is used to represent the operations available at the Final level.
    /// 
    /// Available features (none)
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IQueryCompilerStub<TEntity> : IQuery<TEntity>, IEnumerable<TEntity>
    {

    }
}
