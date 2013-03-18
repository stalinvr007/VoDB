using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VODB.Infrastructure;

namespace VODB.TableToSql
{
    class CountBuilder : ISqlBuilder
    {
        public string Build(ITable table)
        {
            return new StringBuilder("Select count(*) From [").Append(table.Name).Append("] ").ToString();
        }

        public SqlBuilderType BuilderType
        {
            get { return SqlBuilderType.Count; }
        }
    }
}
