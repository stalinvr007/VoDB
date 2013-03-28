using System;
using System.Collections.Generic;
using System.Text;
using VODB.Core.Execution.Executers.DbResults;

namespace VODB.ExpressionsToSql
{

    class BetweenCondition : IQueryCondition
    {
        private ICollection<IQueryParameter> _Parameters;
        private readonly Object _Val1;
        private readonly Object _Val2;

        public BetweenCondition(Object val1, Object val2)
        {
            _Val2 = val2;
            _Val1 = val1;
            _Parameters = new List<IQueryParameter>();
        }

        public string Compile(ref int level)
        {
            return new StringBuilder(" Between ")
                .Append(_Parameters.Add(++level, _Val1))
                .Append(" And ")
                .Append(_Parameters.Add(++level, _Val2))
                .ToString();
        }

        public IEnumerable<IQueryParameter> Parameters
        {
            get { return _Parameters; }
        }
    }
}
