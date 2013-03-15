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

        [TestCaseSource("GetTables")]
        public void ISqlBuilder_Assert_SelectById(ITable table)
        {
            var sql = new SelectByIdBuilder().Build(table);

            StringAssert.StartsWith("Select [", sql);

            foreach (var name in table.Fields.Select(f => f.Name))
            {
                StringAssert.Contains("[" + name + "]", sql);
            }

            StringAssert.Contains(" From [" + table.Name + "]", sql);

            StringAssert.Contains(" Where ", sql);
        }

        [TestCaseSource("GetTables")]
        public void ISqlBuilder_Assert_WhereId(ITable table)
        {
            var sql = new WhereIdBuilder().Build(table);

            StringAssert.StartsWith("Where ", sql);

            foreach (var name in table.Keys.Select(f => f.Name))
            {
                StringAssert.Contains("[" + name + "]", sql);
                StringAssert.Contains("@" + name, sql);
            }
        }

        [TestCaseSource("GetTables")]
        public void ISqlBuilder_Assert_Update(ITable table)
        {
            var sql = new UpdateBuilder().Build(table);

            StringAssert.StartsWith("Update [" + table.Name + "]", sql);

            foreach (var name in table.Fields.Select(f => f.Name))
            {
                StringAssert.Contains("[" + name + "]", sql);
                StringAssert.Contains("@" + name, sql);
            }

            foreach (var name in table.Keys.Select(f => f.Name))
            {
                StringAssert.Contains("[" + name + "]", sql);
                StringAssert.Contains("@" + name, sql);
                StringAssert.Contains("@old" + name, sql);
            }
        }

        [TestCaseSource("GetTables")]
        public void ISqlBuilder_Assert_Insert(ITable table)
        {
            var sql = new InsertBuilder().Build(table);

            StringAssert.StartsWith("Insert into [" + table.Name + "]", sql);

            foreach (var name in table.Fields.Where(f => !f.IsIdentity).Select(f => f.Name))
            {
                StringAssert.Contains("[" + name + "]", sql);
                StringAssert.Contains("@" + name, sql);
            }

        }


    }

}
