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
            public String Name { get; private set; }

            public int Age { get; private set; }
        }

        [TestMethod]
        public void EqualParser()
        {
            var Name = "Sérgio";
            var parser = new ComparatorExpressionParser<Model>();
            
            Assert.AreEqual("Name = @ModelName0", parser.Parse(m => m.Name == Name));
            Assert.AreEqual(1, parser.ConditionData.Count());

        }

        [TestMethod]
        public void EqualParser_ConstVar()
        {
            var parser = new ComparatorExpressionParser<Model>();

            Assert.AreEqual("Name = @ModelName0", parser.Parse(m => m.Name == "Sérgio"));
            Assert.AreEqual(1, parser.ConditionData.Count());
        }

        [TestMethod]
        public void NotEqualParser_ConstVar()
        {
            var parser = new ComparatorExpressionParser<Model>();

            Assert.AreEqual("Name != @ModelName0", parser.Parse(m => m.Name != "Sérgio"));
            Assert.AreEqual(1, parser.ConditionData.Count());
        }

        [TestMethod]
        public void GreaterThanParser_ConstVar()
        {
            var parser = new ComparatorExpressionParser<Model>();

            Assert.AreEqual("Age > @ModelAge0", parser.Parse(m => m.Age > 10));
            Assert.AreEqual(1, parser.ConditionData.Count());
        }

        [TestMethod]
        public void GreaterThanOrEqualParser_ConstVar()
        {
            var parser = new ComparatorExpressionParser<Model>();

            Assert.AreEqual("Age >= @ModelAge0", parser.Parse(m => m.Age >= 10));
            Assert.AreEqual(1, parser.ConditionData.Count());
        }

        [TestMethod]
        public void SmallerThanOrEqualParser_ConstVar()
        {
            var parser = new ComparatorExpressionParser<Model>();

            Assert.AreEqual("Age <= @ModelAge0", parser.Parse(m => m.Age <= 10));
            Assert.AreEqual(1, parser.ConditionData.Count());
        }

        [TestMethod]
        public void SmallerThanParser_ConstVar()
        {
            var parser = new ComparatorExpressionParser<Model>();

            Assert.AreEqual("Age < @ModelAge0", parser.Parse(m => m.Age < 10));
            Assert.AreEqual(1, parser.ConditionData.Count());
        }
        
        [TestMethod]
        public void FieldParser()
        {
            var parser = new FieldGetterExpressionParser<Model, String>();
            Assert.AreEqual("Name", parser.Parse(m => m.Name));
        }
    }
}
