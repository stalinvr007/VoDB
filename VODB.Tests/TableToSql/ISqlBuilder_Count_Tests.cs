using NUnit.Framework;
using VODB.EntityTranslation;
using VODB.Infrastructure;
using VODB.TableToSql;

namespace VODB.Tests.TableToSql
{

    [TestFixture(typeof(EntityTranslator), typeof(CountBuilder))]
    public class ISqlBuilder_Count_Tests<TEntityTranslator, TSqlBuilder> : ISqlBuilderTestBase<TEntityTranslator, TSqlBuilder>
        where TEntityTranslator : IEntityTranslator, new()
        where TSqlBuilder : ISqlBuilder, new()
    {
        [TestCaseSource("GetTables")]
        public void ISqlBuilder_Assert(ISqlBuilder builder, ITable table)
        {
            var sql = builder.Build(table);

            StringAssert.StartsWith("Select count(*) From [" + table.Name + "]", sql);
        }


    }


}
