using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VODB.ExpressionParser.TSqlBuilding
{
    abstract class TSqlBuilderBase : ITSqlBuilder
    {
        protected ICollection<KeyValuePair<Key, Object>> _parameters = new List<KeyValuePair<Key, Object>>();
        protected IExpressionBodyParser _Parser;

        public bool CanBuild(IExpressionBodyParser parser)
        {
            _Parser = parser;
            return CanBuildSql(parser);
        }

        public IEnumerable<KeyValuePair<Key, object>> Parameters
        {
            get { return _parameters; }
        }

        public void ClearParameters()
        {
            _parameters.Clear();
        }

        public abstract string Build(int paramCount);

        protected abstract bool CanBuildSql(IExpressionBodyParser parser);
    }
}
