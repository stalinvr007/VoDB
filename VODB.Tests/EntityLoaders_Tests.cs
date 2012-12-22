using System.Data.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VODB.DbLayer;
using VODB.Core.Loaders;
using VODB.Tests.Models.Northwind;
using VODB.Core.Infrastructure;
using VODB.Core;

namespace VODB.Tests
{
    [TestClass]
    public class EntityLoaders_Tests
    {
        [TestMethod]
        public void EntityKeyLoader_Employees()
        {
            using (DbConnection con = new NameConventionDbConnectionCreator("System.Data.SqlClient").Create())
            {
                con.Open();

                var employee = new Employee();

                var cmd = con.CreateCommand();
                cmd.CommandText = Utils.EmployeeTable.CommandsHolder.Select;

                var reader = cmd.ExecuteReader();

                Assert.IsNotNull(reader);
                Assert.IsTrue(reader.Read());

                new EntityKeyLoader().Load(employee, null, reader);

                reader.Close();

                Assert.AreEqual(1, employee.EmployeeId);
                Assert.IsNull(employee.FirstName);
            }
        }


        [TestMethod]
        public void FullEntityLoader_Employees()
        {
            using (DbConnection con = new NameConventionDbConnectionCreator("System.Data.SqlClient").Create())
            {
                con.Open();

                var employee = new Employee();

                var cmd = con.CreateCommand();
                cmd.CommandText = Utils.EmployeeTable.CommandsHolder.Select;

                var reader = cmd.ExecuteReader();

                Assert.IsNotNull(reader);
                Assert.IsTrue(reader.Read());

                new FullEntityLoader().Load(employee, null, reader);

                reader.Close();

                EntitiesAsserts.Assert_Employee_1(employee);
            }
        }
    }
}