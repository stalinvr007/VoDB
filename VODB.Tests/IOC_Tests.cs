using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VODB.Tests
{
    [TestClass]
    public class IOC_Tests
    {
        [TestMethod]
        public void Session_IOC_Test()
        {
            var session = new Session();
            Assert.IsNotNull(session);
        }
    }
}
