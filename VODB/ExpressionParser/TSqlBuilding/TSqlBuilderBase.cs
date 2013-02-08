using System;
using System.Collections.Generic;

namespace VODB.ExpressionParser.TSqlBuilding
{
    internal abstract class TSqlBuilderBase : ITSqlBuilder
    {
        protected IExpressionBodyParser _Parser;
        protected ICollection<KeyValuePair<Key, Object>> _parameters = new List<KeyValuePair<Key, Object>>();

        #region ITSqlBuilder Members

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

        #endregion

        protected abstract bool CanBuildSql(IExpressionBodyParser parser);
    }
}