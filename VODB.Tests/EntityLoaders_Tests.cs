using System.Data.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VODB.DbLayer;
using VODB.DbLayer.Loaders;
using VODB.Tests.Models.Northwind;
using VODB.VirtualDataBase;

namespace VODB.Tests
{
    [TestClass]
    public class EntityLoaders_Tests
    {
        [TestMethod]
        public void EntityKeyLoader_Employees()
        {
            using (DbConnection con = new DbConnectionCreator("System.Data.SqlClient").Create())
            {
                con.Open();

                var employee = new Employee();

                Table table = employee.Table;
                DbCommand cmd = con.CreateCommand();
                cmd.CommandText = table.CommandsHolder.Select;

                DbDataReader reader = cmd.ExecuteReader();

                Assert.IsNotNull(reader);
                Assert.IsTrue(reader.Read());

                new EntityKeyLoader<Employee>()
                    .Load(employee, reader);

                reader.Close();

                Assert.AreEqual(1, employee.EmployeeId);
                Assert.IsNull(employee.FirstName);
            }
        }


        [TestMethod]
        public void FullEntityLoader_Employees()
        {
            using (DbConnection con = new DbConnectionCreator("System.Data.SqlClient").Create())
            {
                con.Open();

                var employee = new Employee();

                Table table = employee.Table;
                DbCommand cmd = con.CreateCommand();
                cmd.CommandText = table.CommandsHolder.Select;

                DbDataReader reader = cmd.ExecuteReader();

                Assert.IsNotNull(reader);
                Assert.IsTrue(reader.Read());

                new FullEntityLoader<Employee>()
                    .Load(employee, reader);

                reader.Close();

                EntitiesAsserts.Assert_Employee_1(employee);
            }
        }
    }
}