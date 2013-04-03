using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace VODB.ExpressionsToSql
{

    class InCondition<TField> : IQueryCondition
    {

        private ICollection<IQueryParameter> _Parameters;
        private readonly TField[] _Values;
        public InCondition(TField[] values)
        {
            _Values = values;
            _Parameters = new List<IQueryParameter>();
        }

        public InCondition(IEnumerable<TField> values)
        {
            _Values = values.ToArray();
            _Parameters = new List<IQueryParameter>();
        }

        public InCondition(QueryCondition<TField> queryCondition)
        {
            throw new NotImplementedException();
        }

        public string Compile(ref int level)
        {
            var sb = new StringBuilder(" In (");

            foreach (var val in _Values)
            {
                sb.Append(_Parameters.Add(++level, val)).Append(", ");
            }

            sb.Remove(sb.Length - 2, 2).Append(")");
            return sb.ToString();
        }

        public IEnumerable<IQueryParameter> Parameters
        {
            get { return _Parameters; }
        }
    }
}
