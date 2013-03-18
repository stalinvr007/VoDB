using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VODB.Infrastructure;

namespace VODB.TableToSql
{
    class SelectByIdBuilder : ISqlBuilder
    {
        static ISqlBuilder where = new WhereIdBuilder();

        public string Build(ITable table)
        {
            var sb = new StringBuilder("Select ");
            const string LEFT_WRAPPER = "[";
            const string RIGHT_WRAPPER = "], ";

            foreach (var name in table.Fields.Select(f => f.Name))
            {
                sb.Append(LEFT_WRAPPER).Append(name).Append(RIGHT_WRAPPER);
            }

            return sb.Remove(sb.Length - 2, 2).Append(" From [").Append(table.Name).Append("] ")
                .Append(where.Build(table)).ToString();
        }

        public SqlBuilderType BuilderType
        {
            get { return SqlBuilderType.SelectById; }
        }
    }
}