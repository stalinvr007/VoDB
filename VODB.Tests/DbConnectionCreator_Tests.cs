using NUnit.Framework;
using VODB.DbLayer;
using VODB.Exceptions;

namespace VODB.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestFixture]
    public class DbConnectionCreator_Tests
    {
        
        [Test]
        public void Create_A_new_Connection()
        {
            var creator = new DbConnectionCreator("System.Data.SqlClient", "NorthwindSQL");
            var connection = creator.Create();

            Assert.IsNotNull(connection);
        }

        [Test, ExpectedException(typeof(ConnectionStringNotFoundException))]
        public void Create_A_new_Connection_Provider_failed()
        {
            var creator = new DbConnectionCreator("System.Data.Oracle", "NorthwindSQL");
            creator.Create();
        }

        [Test]
        public void Create_A_new_Connection_Unexisting_failed()
        {
            var creator = new DbConnectionCreator("System.Data.SqlClient", "UnexistingSQL");
            var connection = creator.Create();

            Assert.IsNotNull(connection);
        }

        [Test]
        public void Create_A_new_Connection_NoName()
        {
            var creator = new DbConnectionCreator("System.Data.SqlClient");
            var connection = creator.Create();

            Assert.IsNotNull(connection);
        }

        [Test]
        public void Create_A_new_Connection_ConventionBased()
        {
            var creator = new NameConventionDbConnectionCreator("System.Data.SqlClient");
            var connection = creator.Create();

            Assert.IsNotNull(connection);
        }


    }
}
