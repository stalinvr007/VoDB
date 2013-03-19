using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VODB.EntityMapping;
using VODB.Infrastructure;

namespace VODB.Tests.Mapping
{
    [TestFixture(typeof(OrderedEntityMapper))]
    public class EntityMapper_Tests<TEntityMapper> where TEntityMapper : IEntityMapper, new()
    {

        private static IEnumerable GetTables()
        {
            return Utils.TestModels
                .ToTables()
                .Where(t => t.Name != "CustomerCustomerDemo")
                .Where(t => t.Name != "CustomerDemographics");
        }

        [TestCaseSource("GetTables")]
        public void EntityMapper_Assert(ITable table)
        {
            Utils.ExecuteWith(con =>
            {
                var cmd = con.CreateCommand();
                cmd.CommandText = table.SqlSelect;

                var reader = cmd.ExecuteReader();
                
                Assert.That(reader.Read(), Is.True);

                var mapper = new TEntityMapper();

                var entity = mapper.Map(
                    Activator.CreateInstance(table.EntityType), 
                    table, 
                    reader
                );

            });
        }

    }
}
