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

                Assert.AreEqual(1, employee.EmployeeId);
                Assert.AreEqual("Nancy", employee.FirstName);
                Assert.AreEqual("Davolio", employee.LastName);
                Assert.AreEqual("Sales Representative", employee.Title);
                Assert.AreEqual("Ms.", employee.TitleOfCourtesy);
                Assert.AreEqual(new DateTime(1948,12,8), employee.BirthDate);
                Assert.AreEqual(new DateTime(1992,5,1), employee.HireDate);
                Assert.AreEqual("507 - 20th Ave. E.\r\nApt. 2A", employee.Address);
                Assert.AreEqual("Seattle", employee.City);
                Assert.AreEqual("WA", employee.Region);
                Assert.AreEqual("98122", employee.PostalCode);
                Assert.AreEqual("USA", employee.Country);
                Assert.AreEqual("(206) 555-9857", employee.HomePhone);
                Assert.AreEqual("5467", employee.Extension);
                Assert.AreEqual(21626, employee.Photo.Count());
                Assert.AreEqual("Education includes a BA in psychology from Colorado State University in 1970.  She also completed \"The Art of the Cold Call.\"  Nancy is a member of Toastmasters International.", employee.Notes);
                Assert.AreEqual("http://accweb/emmployees/davolio.bmp", employee.PhotoPath);
                Assert.IsNotNull( employee.ReportsTo);
                Assert.AreEqual(2, employee.ReportsTo.EmployeeId);
                
            }

        }

    }
}
