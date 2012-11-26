using System;
using System.Linq;
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
            var Name = "Sérgio";
            var parser = new ComparatorExpressionParser<Model>();
            
            Assert.AreEqual("Name = @ModelName", parser.Parse(m => m.Name == Name));
            Assert.AreEqual(1, parser.ConditionData.Count());

        }
    }
}
