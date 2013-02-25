using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VODB.Core.Infrastructure;
using VODB.ExpressionParser;

namespace VODB.ExpressionsToSql
{

    class Query<TEntity>
    {

        private readonly IExpressionDecoder _Expression;

        public Query(Expression<Func<TEntity, Boolean>> expression)
        {
            _Expression = new ExpressionDecoder<TEntity>(expression);
        }

        public String Compile(int level)
        {
            var sb = new StringBuilder();

            var parts = _Expression.DecodeLeft().ToList();
            
            sb.AppendFormat("{0} = {1}{2}", parts[0].Field.FieldName, "@p", level);


            return sb.ToString();
        }

        

        private void Append(StringBuilder sb, IList<ExpressionPart> parts, int index)
        {
            

        }

    }

}
