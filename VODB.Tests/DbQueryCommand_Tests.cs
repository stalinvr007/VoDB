using Microsoft.VisualStudio.TestTools.UnitTesting;
using VODB.DbLayer;
using System.Linq;
using VODB.DbLayer.DbExecuters;
using VODB.Tests.Models.Northwind;
using VODB.DbLayer.Loaders;
using System;
using VODB.DbLayer.DbCommands;

namespace VODB.Tests
{
    [TestClass]
    public class DbQueryCommand_Tests
    {
        [TestMethod]
        public void GetEmployeesData()
        {
            using (var con = new DbConnectionCreator("System.Data.SqlClient").Create())
            {
                con.Open();

                var table = new Employee().Table;
                var cmd = con.CreateCommand();
                cmd.CommandText = table.CommandsHolder.Select;

                var query = new DbQueryExecuterCommandEager(cmd, table);

                Assert.AreEqual(9, query.Execute().Count());

                con.Close();
            }

        }


        [TestMethod]
        public void GetEmployeesEntities()
        {
            using (var con = new DbConnectionCreator("System.Data.SqlClient").Create())
            {
                con.Open();

                var factory = new DbEntitySelectCommandFactory<Employee>(con);

                var query = new DbEntityQueryExecuterEager<Employee>(factory, new FullEntityLoader<Employee>());

                var result = query.Execute();

                Assert.AreEqual(9, result.Count());

                Assert.AreEqual(9, result.Where(emp => !String.IsNullOrEmpty(emp.FirstName)).Count());

                con.Close();
            }

        }

        [TestMethod]
        public void GetEmployeesEntities_Lazy()
        {
            using (var con = new DbConnectionCreator("System.Data.SqlClient").Create())
            {
                con.Open();

                var factory = new DbEntitySelectCommandFactory<Employee>(con);

                var query = new DbEntityQueryExecuterLazy<Employee>(factory, new FullEntityLoader<Employee>());

                var result = query.Execute();
                
                Assert.AreEqual(9, result.Where(emp => !String.IsNullOrEmpty(emp.FirstName)).Count());

                con.Close();
            }

        }

        [TestMethod]
        public void GetEmployeesById_Eager()
        {
            using (var con = new DbConnectionCreator("System.Data.SqlClient").Create())
            {
                con.Open();

                var factory = new DbEntitySelectByIdCommandFactory<Employee>(con,
                    new Employee { EmployeeId = 1 });

                var query = new DbEntityQueryExecuterEager<Employee>(factory, new FullEntityLoader<Employee>());

                var result = query.Execute();

                Assert.AreEqual(1, result.Count());

                EntitiesAsserts.Assert_Employee_1(result.First());

                con.Close();
            }

        }
    }
}
