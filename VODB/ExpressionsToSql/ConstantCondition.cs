using System;
using System.Collections.Generic;
using System.Text;

namespace VODB.ExpressionsToSql
{
    class ConstantCondition : IQueryCondition
    {
        static IEnumerable<IQueryParameter> PARAMETERS = new List<IQueryParameter>();
        private readonly String _Condition;
        public ConstantCondition(String condition)
        {
            _Condition = condition;
        }

        public string Compile(ref int level)
        {
            return _Condition + ++level;
        }

        public IEnumerable<IQueryParameter> Parameters
        {
            get { return PARAMETERS; }
        }
    }
}