using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VODB.Infrastructure;

namespace VODB.TableToSql
{
    class DeleteByIdBuilder : SqlBuilderBaseById
    {
        

        public DeleteByIdBuilder() : base(SqlBuilderType.CountById) { }

        public override string Build(ITable table)
        {
            return new StringBuilder("Delete From [").Append(table.Name).Append("] ")
                .Append(Where.Build(table)).ToString();
        }

    }
}
