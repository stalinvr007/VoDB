using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VODB.Infrastructure;

namespace VODB.TableToSql
{
    class WhereIdBuilder : ISqlBuilder
    {

        private readonly bool _WithOldValues;
        public WhereIdBuilder(bool withOldValues = false)
        {
            _WithOldValues = withOldValues;
        }

        public string Build(ITable table)
        {
            var sb = new StringBuilder("Where ");
            const string LEFT_WRAPPER = "[";
            const string RIGHT_WRAPPER = "]";
            const string WHITE_SPACE = " ";

            foreach (var name in table.Keys.Select(f => f.Name))
            {
                sb.Append(LEFT_WRAPPER).Append(name).Append(RIGHT_WRAPPER)
                    .Append(" = @");
                
                if (_WithOldValues)
                {
                    sb.Append("old");
                }

                sb.Append(name.Replace(WHITE_SPACE, "")).Append(" and ");
            }

            return sb.Remove(sb.Length - 5, 5).ToString();
        }

        public SqlBuilderType BuilderType
        {
            get { return SqlBuilderType.WhereById; }
        }
    }
}
