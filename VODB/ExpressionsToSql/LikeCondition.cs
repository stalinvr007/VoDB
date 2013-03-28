using System;
using System.Collections.Generic;
using System.Text;
using VODB.Core.Execution.Executers.DbResults;

namespace VODB.ExpressionsToSql
{
    class LikeCondition : IQueryCondition
    {
        private ICollection<IQueryParameter> _Parameters;

        private readonly String _Value;
        private readonly WildCard _Token;

        public LikeCondition(String value, WildCard token)
        {
            _Token = token;
            _Value = value;
            _Parameters = new List<IQueryParameter>();
        }

        public string Compile(ref int level)
        {
            var sb = new StringBuilder(" Like ");
            if (_Token == WildCard.Left || _Token == WildCard.Both)
            {
                sb.Append("'%' + ");
            }
            _Parameters.Add(new QueryParameter
            {
                Name = "@p" + ++level,
                Value = _Value
            });

            sb.Append("@p" + level);

            if (_Token == WildCard.Right || _Token == WildCard.Both)
            {
                sb.Append(" + '%'");
            }

            return sb.ToString();
        }

        public IEnumerable<IQueryParameter> Parameters
        {
            get { return _Parameters; }
        }
    }
}
