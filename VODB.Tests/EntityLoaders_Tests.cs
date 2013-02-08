using System.Data.Common;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using VODB.DbLayer;
using VODB.Core.Loaders;
using VODB.Tests.Models.Northwind;

namespace VODB.Tests
{
    [TestFixture]
    public class EntityLoaders_Tests
    {
        [Test]
        public void EntityKeyLoader_Employees()
        {
            using (var con = new NameConventionDbConnectionCreator("System.Data.SqlClient").Create())
            {
                con.Open();

                var employee = new Employee();

                var cmd = con.CreateCommand();
                cmd.CommandText = Utils.EmployeeTable.CommandsHolder.Select;

                var reader = cmd.ExecuteReader();

                Assert.IsNotNull(reader);
                Assert.IsTrue(reader.Read());

                new EntityKeyLoader(new CachedEntities()).Load(employee, null, reader);

                reader.Close();

                Assert.AreEqual(1, employee.EmployeeId);
                Assert.IsNull(employee.FirstName);
            }
        }


        [Test]
        public void FullEntityLoader_Employees()
        {
            using (var con = new NameConventionDbConnectionCreator("System.Data.SqlClient").Create())
            {
                con.Open();

                var employee = new Employee();

                var cmd = con.CreateCommand();
                cmd.CommandText = Utils.EmployeeTable.CommandsHolder.Select;

                var reader = cmd.ExecuteReader();

                Assert.IsNotNull(reader);
                Assert.IsTrue(reader.Read());

                new FullEntityLoader(new CachedEntities()).Load(employee, null, reader);

                reader.Close();

                EntitiesAsserts.Assert_Employee_1(employee);
            }
        }

        [Test]
        public void ForeignKey_Setter()
        {

            Utils.Execute(session =>
            {
                var entity1 = session.GetById(new Employee { EmployeeId = 1 });
                var entity2 = session.GetById(new Employee { EmployeeId = 2 });

                entity1.ReportsTo = entity2.ReportsTo;

                Assert.AreEqual(entity1.ReportsTo.EmployeeId, entity2.ReportsTo.EmployeeId);
            });

        }

        [Test]
        public void DbReaderMapper()
        {
            Utils.ExecuteWith(con =>
            {
                var cmd = con.CreateCommand();
                cmd.CommandText = Utils.EmployeeTable.CommandsHolder.Select;

                var reader = cmd.ExecuteReader();

                var employees = new DbReaderMapper(new DictionaryMapper())
                     .Map<Employee>(reader);

                while (!employees.IsCompleted)
                {
                    Thread.Sleep(0);
                }

                Assert.AreEqual(9, employees.Result.Count());

            });


        }

    }
}