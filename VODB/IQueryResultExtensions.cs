using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using VODB.DbLayer.DbResults;

namespace VODB
{
    public static class IQueryResultExtensions
    {

        public static IEnumerable<TEntity> In<TEntity, TIn>(this IDbQueryResult<TEntity> result, IEnumerable<TIn> inList, Func<TIn, Expression<Func<TEntity, Boolean>>> condition)
        {
            foreach (var inItem in inList)
            {
                foreach (var resItem in result.Where(condition(inItem)))
                {
                    yield return resItem;
                }
            }
        }

    }
}
