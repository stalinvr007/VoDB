using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VODB.DbLayer;
using VODB.DbLayer.Exceptions;

namespace VODB.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class DbConnectionCreator_Tests
    {
        
        [TestMethod]
        public void Create_A_new_Connection()
        {
            var creator = new DbConnectionCreator("System.Data.SqlClient", "NorthwindSQL");
            var connection = creator.Create();

            Assert.IsNotNull(connection);
        }

        [TestMethod, ExpectedException(typeof(ConnectionStringNotFoundException))]
        public void Create_A_new_Connection_Provider_failed()
        {
            var creator = new DbConnectionCreator("System.Data.Oracle", "NorthwindSQL");
            var connection = creator.Create();

        }

        [TestMethod]
        public void Create_A_new_Connection_Unexisting_failed()
        {
            var creator = new DbConnectionCreator("System.Data.SqlClient", "UnexistingSQL");
            var connection = creator.Create();

            Assert.IsNotNull(connection);
        }

        [TestMethod]
        public void Create_A_new_Connection_NoName()
        {
            var creator = new DbConnectionCreator("System.Data.SqlClient");
            var connection = creator.Create();

            Assert.IsNotNull(connection);
        }

    }
}
