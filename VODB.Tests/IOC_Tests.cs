using NUnit.Framework;
using VODB.Sessions;

namespace VODB.Tests
{
    [TestFixture]
    public class IOC_Tests
    {
        [Test]
        public void Session_IOC_Test()
        {
            var session = new SessionV1();
            Assert.IsNotNull(session);
        }
    }
}
