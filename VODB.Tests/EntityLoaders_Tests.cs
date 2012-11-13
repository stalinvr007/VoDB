using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VODB.DbLayer;
using VODB.Tests.Models.Northwind;
using System.Linq;

namespace VODB.Tests
{
    [TestClass]
    public class EntityLoaders_Tests
    {


        [TestMethod]
        public void EntityKeyLoader_Employees()
        {

            using (var con = new DbConnectionCreator("System.Data.SqlClient").Create())
            {
                con.Open();

                Employee employee = new Employee();

                var table = employee.Table;
                var cmd = con.CreateCommand();
                cmd.CommandText = table.CommandsHolder.Select;

                var reader = cmd.ExecuteReader();

                Assert.IsNotNull(reader);
                Assert.IsTrue(reader.Read());

                new VODB.DbLayer.Loaders.EntityKeyLoader<Employee>()
                    .Load(employee, reader);

                reader.Close();

                Assert.AreEqual(1, employee.EmployeeId);
                Assert.IsNull(employee.FirstName);

            }

        }

       

        [TestMethod]
        public void FullEntityLoader_Employees()
        {

            using (var con = new DbConnectionCreator("System.Data.SqlClient").Create())
            {
                con.Open();

                Employee employee = new Employee();

                var table = employee.Table;
                var cmd = con.CreateCommand();
                cmd.CommandText = table.CommandsHolder.Select;

                var reader = cmd.ExecuteReader();

                Assert.IsNotNull(reader);
                Assert.IsTrue(reader.Read());

                new VODB.DbLayer.Loaders.FullEntityLoader<Employee>()
                    .Load(employee, reader);

                reader.Close();

                EntitiesAsserts.Assert_Employee_1(employee);
                
            }

        }

    }
}
