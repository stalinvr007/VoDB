using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace VODB.ExpressionsToSql
{
    class OrderByCondition<TEntity> : ParameterLessCondition
    {
        private static StubCondition Stub = new StubCondition();
        private readonly QueryCondition<TEntity> queryCondition;

        public OrderByCondition(Expression<Func<TEntity, Object>> expression)
        {
             queryCondition = new QueryCondition<TEntity>(expression, Stub);
        }

        public override string Compile(ref int level)
        {
            return " Order By " + queryCondition.Compile(ref level);
        }
    }
}
