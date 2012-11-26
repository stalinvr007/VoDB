using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VODB.ExpressionParser;

namespace VODB.Tests
{
    [TestClass]
    public class ExpressionParser_Tests
    {
        sealed class Model : DbEntity
        {
            public String Name { get; set; }
        }

        [TestMethod]
        public void TestMethod2()
        {
            const string Name = "Sérgio";

            Assert.AreEqual("Name = 'Sérgio'", new ComparatorExpressionParser<Model>()
                .Parse(m => m.Name == Name));
        }
    }
}
