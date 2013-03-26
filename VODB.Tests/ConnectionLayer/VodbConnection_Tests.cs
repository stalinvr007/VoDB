using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VODB.Sessions;

namespace VODB.Tests.ConnectionLayer
{
    [TestFixture]
    public class VodbConnection_Tests
    {

        [Test]
        public void Connection_Close_NoOpen()
        {
            var connection = new VodbConnection(Utils.ConnectionCreator);
            connection.Close();
            Assert.That(connection.IsOpened, Is.False);
        }

        [Test]
        public void Connection_IsOpened()
        {
            var connection = new VodbConnection(Utils.ConnectionCreator);
            connection.Open();
            Assert.That(connection.IsOpened, Is.True);
            connection.Close();
            Assert.That(connection.IsOpened, Is.False);
        }

        [Test]
        public void Connection_IsNotOpen_After_Dispose()
        {
            VodbConnection connection;
            using (connection = new VodbConnection(Utils.ConnectionCreator))
            {
                connection.Open();
                Assert.That(connection.IsOpened, Is.True);
            }
            Assert.That(connection.IsOpened, Is.False);
        }

        [Test]
        public void Connection_Reopen()
        {
            var connection = new VodbConnection(Utils.ConnectionCreator);
            for (int i = 0; i < 10; i++)
            {
                connection.Open();
                Assert.That(connection.IsOpened, Is.True);
                connection.Close();
                Assert.That(connection.IsOpened, Is.False);
            }
        }

    }
}
