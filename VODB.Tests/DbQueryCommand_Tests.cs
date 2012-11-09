using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VODB.DbLayer;
using VODB.Tests.Models;
using System.Linq;

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

                var table = new Employees().Table;
                var cmd = con.CreateCommand();
                cmd.CommandText = table.CommandsHolder.Select;

                var query = new DbQueryCommand(cmd, table);

                Assert.AreEqual(9, query.Query().Count());


            }

        }
    }
}
