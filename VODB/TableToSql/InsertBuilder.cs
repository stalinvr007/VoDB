using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VODB.Infrastructure;

namespace VODB.TableToSql
{
    class InsertBuilder : ISqlBuilder
    {

        public string Build(ITable table)
        {
            var sb = new StringBuilder("Insert into [").Append(table.Name).Append("] (");
            const string LEFT_WRAPPER = "[";
            const string RIGHT_WRAPPER = "], ";
            const string WHITE_SPACE = " ";

            foreach (var name in table.Fields.Where(f => !f.IsIdentity).Select(f => f.Name))
            {
                sb.Append(LEFT_WRAPPER).Append(name).Append(RIGHT_WRAPPER);
            }

            sb.Remove(sb.Length - 2, 2).Append(") values (");

            foreach (var name in table.Fields.Where(f => !f.IsIdentity).Select(f => f.Name))
            {
                sb.Append("@").Append(name.Replace(WHITE_SPACE, "")).Append(", ");
            }

            return sb.Remove(sb.Length - 2, 2).ToString();
        }
    }
}
