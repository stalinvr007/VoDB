using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VODB.EntityTranslation;
using VODB.Infrastructure;
using VODB.TableToSql;
using VODB.Tests.Models.Northwind;

namespace VODB.Tests.TableToSql
{
    [TestFixture(typeof(EntityTranslator), typeof(WhereIdBuilder))]
    public class ISqlBuilder_WhereId_Tests<TEntityTranslator, TSqlBuilder> : ISqlBuilderTestBase<TEntityTranslator, TSqlBuilder>
        where TEntityTranslator : IEntityTranslator, new()
        where TSqlBuilder : ISqlBuilder, new()
    {
        [TestCaseSource("GetTables")]
        public void ISqlBuilder_Assert(ISqlBuilder builder, ITable table)
        {
            var sql = builder.Build(table);

            StringAssert.StartsWith("Where ", sql);

            foreach (var name in table.Keys.Select(f => f.Name))
            {
                StringAssert.Contains("[" + name + "]", sql);
                StringAssert.Contains("@" + name, sql);
            }
        }


    }
}
