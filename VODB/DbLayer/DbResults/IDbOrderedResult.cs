using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq.Expressions;
using VODB.DbLayer.DbCommands;
using VODB.Exceptions;

namespace VODB.DbLayer.DbResults
{

    public interface IDbOrderedResult<TEntity> : IEnumerable<TEntity>
    {

        IDbOrderedDescResult<TEntity> Descending();

    }
}
