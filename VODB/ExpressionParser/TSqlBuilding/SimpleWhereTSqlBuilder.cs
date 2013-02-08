using System;
using System.Collections.Generic;
using System.Linq;
using VODB.Core;
using VODB.Exceptions;

namespace VODB.ExpressionParser.TSqlBuilding
{
    internal class SimpleWhereTSqlBuilder : TSqlBuilderBase
    {
        private static readonly IConfiguration Configuration = Engine.Get<IConfiguration>();

        protected override Boolean CanBuildSql(IExpressionBodyParser parser)
        {
            return !parser.IsComplex;
        }

        public override String Build(int paramCount)
        {
            string paramName = String.Format("{0}{1}", _Parser.Field.FieldName, paramCount);
            _parameters.Add(new KeyValuePair<Key, object>(new Key(_Parser.Field, paramName), _Parser.Value));

            IWhereExpressionFormatter formatter =
                Configuration.WhereExpressionFormatters.First(w => w.CanFormat(_Parser.NodeType));

            if (formatter == null)
            {
                throw new WhereExpressionFormatterNotFoundException();
            }

            return formatter.Format(_Parser.Field.FieldName, paramName);
        }
    }
}