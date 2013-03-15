using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VODB.Infrastructure;

namespace VODB.TableToSql
{
    class CountByIdBuilder : ISqlBuilder
    {
        static ISqlBuilder where = new WhereIdBuilder();

        public string Build(ITable table)
        {
            return new StringBuilder("Select count(*) From [").Append(table.Name).Append("] ")
                .Append(where.Build(table)).ToString();
        }
    }
}
