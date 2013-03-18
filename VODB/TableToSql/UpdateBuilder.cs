using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VODB.Infrastructure;

namespace VODB.TableToSql
{
    class UpdateBuilder : ISqlBuilder
    {
        static ISqlBuilder where = new WhereIdBuilder(true);

        public string Build(ITable table)
        {
            var sb = new StringBuilder("Update [").Append(table.Name).Append("]");
            const string LEFT_WRAPPER = "[";
            const string RIGHT_WRAPPER = "]";
            const string WHITE_SPACE = " ";

            foreach (var name in table.Fields.Select(f => f.Name))
            {
                
                sb.Append(LEFT_WRAPPER).Append(name).Append(RIGHT_WRAPPER)
                    .Append(" = @").Append(name.Replace(WHITE_SPACE, "")).Append(", ");
            }

            return sb.Remove(sb.Length - 2, 2)
                .Append(where.Build(table))
                .ToString();
        }

        public SqlBuilderType BuilderType
        {
            get { return SqlBuilderType.Update; }
        }
    }
}