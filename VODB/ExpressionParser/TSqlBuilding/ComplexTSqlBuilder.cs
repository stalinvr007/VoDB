using System;
using VODB.Core.Infrastructure;

namespace VODB.ExpressionParser.TSqlBuilding
{

    class ComplexTSqlBuilder : TSqlBuilderBase
    {

        public override string Build(int paramCount)
        {
            return Build(paramCount, _Parser);
        }

        private string Build(int paramCount, IExpressionBodyParser parser)
        {
            if (parser != null)
            {
                if (!parser.IsComplex)
                {
                    var table = parser.Entity.GetTable();

                    Field field = null;
                    if (parser.Field.BindedTo != null)
                    {
                        field = table.FindFieldByBind(parser.Field.BindedTo);
                    }

                    return String.Format("{0} In (Select {1} From {2}",
                        parser.Field.FieldName,
                        field != null ? parser.Field.BindedTo : parser.Field.FieldName,
                        table.TableName);
                }

                var simple = new SimpleWhereTSqlBuilder();
                simple.CanBuild(parser);

                var result = String.Format(" Where {0} )", simple.Build(paramCount));

                foreach (var param in simple.Parameters)
                {
                    _parameters.Add(param);
                }
                return Build(paramCount, parser.Next) + result;
            }

            return "";
        }

        protected override bool CanBuildSql(IExpressionBodyParser parser)
        {
            return parser.IsComplex;
        }
    }
}
