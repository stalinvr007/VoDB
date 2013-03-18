using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VODB.Infrastructure;

namespace VODB.TableToSql
{
    class WhereIdBuilder : SqlBuilderBase
    {

        private readonly bool _WithOldValues;
        public WhereIdBuilder() : this(false) { }

        public WhereIdBuilder(bool withOldValues = false) : base(SqlBuilderType.WhereById)
        {
            _WithOldValues = withOldValues;
        }

        public override string Build(ITable table)
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
    }
}
