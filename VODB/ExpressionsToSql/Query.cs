using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VODB.ExpressionParser;

namespace VODB.ExpressionsToSql
{

    class Query<TEntity>
    {

        private readonly IExpressionBodyParser _Expression;

        public Query(Expression<Func<TEntity, Boolean>> expression)
        {
            _Expression = new ExpressionBodyParser(expression);
        }

        public String Compile()
        {

            return "";
        }

    }

}
