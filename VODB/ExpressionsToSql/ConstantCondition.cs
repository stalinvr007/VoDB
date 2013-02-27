using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VODB.Core.Infrastructure;
using VODB.ExpressionParser;
using VODB.Expressions;

namespace VODB.ExpressionsToSql
{
    class ConstantCondition : IQueryCondition
    {

        private readonly String _Condition;
        public ConstantCondition(String condition)
        {
            _Condition = condition;
        }

        public string Compile(int level)
        {
            return _Condition + level;
        }

        public IEnumerable<IQueryParameter> Parameters
        {
            get { throw new NotImplementedException(); }
        }
    }
}
