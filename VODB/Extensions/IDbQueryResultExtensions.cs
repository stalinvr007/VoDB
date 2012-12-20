using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using VODB.Core.Execution.Executers.DbResults;

namespace VODB
{
    /// <summary>
    /// Enables the users that use linq to identify with ease what operations are allowed in T-SQL.
    /// </summary>
    public static class IDbQueryResultExtensions
    {

        public static IDbAndQueryResult<TEntity> TSqlWhere<TEntity>(this IDbQueryResult<TEntity> result, Expression<Func<TEntity, Boolean>> condition)
        {
            return result.Where(condition);
        }

        public static IDbFieldFilterResult<TEntity> TSqlWhere<TEntity, TField>(this IDbQueryResult<TEntity> result, Expression<Func<TEntity, TField>> field)
        {
            return result.Where(field);
        }

        public static IDbOrderedResult<TEntity> TSqlOrderBy<TEntity, TField>(this IDbQueryResult<TEntity> result, Expression<Func<TEntity, TField>> orderByField)
        {
            return result.OrderBy(orderByField);
        }

        public static IDbAndQueryResult<TEntity> TSqlAnd<TEntity>(this IDbAndQueryResult<TEntity> result, Expression<Func<TEntity, Boolean>> condition)
        {
            return result.And(condition);
        }

        public static IDbFieldFilterResult<TEntity> TSqlAnd<TEntity, TField>(this IDbAndQueryResult<TEntity> result, Expression<Func<TEntity, TField>> field)
        {
            return result.And(field);
        }

        public static IDbOrderedResult<TEntity> TSqlOrderBy<TEntity, TField>(this IDbAndQueryResult<TEntity> result, Expression<Func<TEntity, TField>> orderByField)
        {
            return result.OrderBy(orderByField);
        }

        public static IDbOrderedDescResult<TEntity> TSqlDescending<TEntity>(this IDbOrderedResult<TEntity> result)
        {
            return result.Descending();
        }

        public static IDbAndQueryResult<TEntity> TSqlIn<TEntity, TField>(this IDbFieldFilterResult<TEntity> result, IEnumerable<TField> collection)
        {
            return result.In(collection);
        }

        public static IDbAndQueryResult<TEntity> TSqlBetween<TEntity, TField>(this IDbFieldFilterResult<TEntity> result, TField firstValue, TField secondValue)
        {
            return result.Between(firstValue, secondValue);
        }
    }
}
