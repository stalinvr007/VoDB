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

    class InCondition : IQueryCondition
    {

        private ICollection<IQueryParameter> _Parameters;
        private readonly Object[] _Values;
        public InCondition(Object[] values)
        {
            _Values = values;
            _Parameters = new List<IQueryParameter>();
        }

        public string Compile(ref int level)
        {
            var sb = new StringBuilder(" In (");

            foreach (var val in _Values)
            {
                sb.Append("@p").Append(++level).Append(", ");

                _Parameters.Add(new QueryParameter
                {
                    Name = "@p" + level,
                    Value = val
                });
            }

            sb.Remove(sb.Length-2, 2).Append(")");
            return sb.ToString();
        }

        public IEnumerable<IQueryParameter> Parameters
        {
            get { return _Parameters; }
        }
    }
}
