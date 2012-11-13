using Microsoft.VisualStudio.TestTools.UnitTesting;
using VODB.DbLayer;
using System.Linq;
using VODB.DbLayer.DbExecuters;
using VODB.Tests.Models.Northwind;
using VODB.DbLayer.Loaders;
using System;

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

                var query = new DbEntityQueryExecuterEager<Employee>(con, new FullEntityLoader<Employee>());

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

                var query = new DbEntityQueryExecuterLazy<Employee>(con, new FullEntityLoader<Employee>());

                var result = query.Execute();
                
                Assert.AreEqual(9, result.Where(emp => !String.IsNullOrEmpty(emp.FirstName)).Count());

                con.Close();
            }

        }
    }
}
