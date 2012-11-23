using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq.Expressions;
using VODB.ExpressionParser;

namespace VODB.Tests
{
    [TestClass]
    public class ExpressionParser_Tests
    {
        class Model : DbEntity
        {
            public String Name { get; set; }
        }

        [TestMethod]
        public void TestMethod2()
        {
            String Name = "Sérgio";
            
            Assert.AreEqual("Name = 'Sérgio'", new ComparatorExpressionParser<Model>()
                .Parse(m => m.Name == Name));
        }

    }
}
