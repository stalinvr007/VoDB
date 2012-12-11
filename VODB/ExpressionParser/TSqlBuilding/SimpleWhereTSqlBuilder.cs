using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VODB.Exceptions;

namespace VODB.ExpressionParser.TSqlBuilding
{
    public class SimpleWhereTSqlBuilder : ITSqlBuilder
    {
        ICollection<KeyValuePair<Key, Object>> _parameters = new List<KeyValuePair<Key, Object>>();
        private IExpressionBodyParser _Parser;
        
        /// <summary>
        /// Determines whether this instance can build the Sql using the specified parser.
        /// </summary>
        /// <param name="parser">The parser.</param>
        /// <returns></returns>
        public Boolean CanBuild(IExpressionBodyParser parser)
        {
            _Parser = parser;
            return !parser.IsComplex;
        }

        /// <summary>
        /// Gets the parameters used on the Sql Statement.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        public IEnumerable<KeyValuePair<Key, Object>> Parameters
        {
            get { return _parameters; }
        }
        
        /// <summary>
        /// Builds Sql.
        /// </summary>
        /// <returns></returns>
        public String Build()
        {
            _parameters.Add(new KeyValuePair<Key, object>(new Key(_Parser.Field, ""), _Parser.Value));

            var formatter = Configuration.WhereExpressionFormatters.First(w => w.CanFormat(_Parser.NodeType));

            if (formatter == null)
            {
                throw new WhereExpressionFormatterNotFoundException();
            }

            return formatter.Format(_Parser.Field.FieldName, "");
        }
    }
}
