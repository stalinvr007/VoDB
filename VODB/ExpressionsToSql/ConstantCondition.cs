using System;
using System.Collections.Generic;

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
