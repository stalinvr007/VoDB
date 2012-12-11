using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
                if (typeof(Entity).IsAssignableFrom(parser.Field.FieldType))
                {
                    return String.Format("{0} In (Select {1} From {2}",
                        parser.Field.FieldName,
                        parser.Field.BindedTo ?? parser.Field.FieldName,
                        parser.Entity.Table.TableName);
                }
                else
                {
                    var simple = new SimpleWhereTSqlBuilder();
                    simple.CanBuild(parser);

                    string result = String.Format(" Where {0} )", simple.Build(paramCount));

                    foreach (var param in simple.Parameters)
                    {
                        _parameters.Add(param);
                    }
                    return Build(paramCount, parser.BodyParser) + result;
                }
            }

            return "";
        }

        protected override bool CanBuildSql(IExpressionBodyParser parser)
        {
            return parser.IsComplex;
        }
    }
}
