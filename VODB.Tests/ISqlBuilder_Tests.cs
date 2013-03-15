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

namespace VODB.Tests
{

    [TestFixture(typeof(EntityTranslator))]
    public class ISqlBuilder_Tests<TEntityTranslator> where TEntityTranslator : IEntityTranslator, new()
    {
        IEntityTranslator translator = new TEntityTranslator();

        private IEnumerable GetTables()
        {
            return Utils.TestModels
                .ToTables(translator)
                .Select(t => new TestCaseData(t));
        }

        [TestCaseSource("GetTables")]
        public void ISqlBuilder_Assert_Select(ITable table)
        {
            var sql = new SelectBuilder().Build(table);

            StringAssert.StartsWith("Select [", sql);

            foreach (var name in table.Fields.Select(f => f.Name))
            {
                StringAssert.Contains("[" + name + "]", sql);
            }

            StringAssert.EndsWith(" From [" + table.Name + "]", sql);
        }


    }

}
