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
    /// This interface is used to represent the operations available at the third level.
    /// 
    /// Available features
    /// And, Or, OrderBy
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IQueryCompilerLevel3<TEntity> : IEnumerable<TEntity>
    {
        /// <summary>
        /// Afects the Order By clause with Descending flag.
        /// </summary>
        /// <returns></returns>
        IQueryCompilerStub<TEntity> Descending();
    }
}
