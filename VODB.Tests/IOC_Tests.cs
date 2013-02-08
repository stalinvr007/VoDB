using NUnit.Framework;

namespace VODB.Tests
{
    [TestFixture]
    public class IOC_Tests
    {
        [Test]
        public void Session_IOC_Test()
        {
            var session = new Session();
            Assert.IsNotNull(session);
        }
    }
}
