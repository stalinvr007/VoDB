using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VODB.EntityTranslation;
using VODB.Exceptions;

namespace VODB.ExpressionsToSql
{
    class OrderByCondition<TEntity> : ParameterLessCondition
    {
        private static StubCondition Stub = new StubCondition();
        private readonly QueryCondition<TEntity> queryCondition;

        public OrderByCondition(IEntityTranslator translator, Expression<Func<TEntity, Object>> expression)
        {
            queryCondition = new QueryCondition<TEntity>(translator, expression, Stub);
        }

        public override string Compile(ref int level)
        {
            var result = queryCondition.Compile(ref level);
            if (result.Contains(" in ("))
            {
                throw new OrderByClauseException("The order by clause can't have more than one level.");
            }
            return " Order By " + result;
        }
    }
}
