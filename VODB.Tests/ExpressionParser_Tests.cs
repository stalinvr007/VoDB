using System;
using System.Linq;
using NUnit.Framework;
using VODB.ExpressionParser;

namespace VODB.Tests
{
    [TestFixture]
    public class ExpressionParser_Tests
    {
        public sealed class Model
        {
            public String Name { get; private set; }

            public int Age { get; private set; }
        }

        [Test]
        public void EqualParser()
        {
            var Name = "Sérgio";
            var parser = new ComparatorExpressionParser<Model>();

            StringAssert.Contains(parser.Parse(m => m.Name == Name), "Name = @Name0");
            Assert.AreEqual(1, parser.ConditionData.Count());

        }

        [Test]
        public void EqualParser_ConstVar()
        {
            var parser = new ComparatorExpressionParser<Model>();

            StringAssert.Contains(parser.Parse(m => m.Name == "Sérgio"), "Name = @Name0");
            Assert.AreEqual(1, parser.ConditionData.Count());
        }

        [Test]
        public void NotEqualParser_ConstVar()
        {
            var parser = new ComparatorExpressionParser<Model>();

            StringAssert.Contains(parser.Parse(m => m.Name != "Sérgio"), "Name != @Name0");
            Assert.AreEqual(1, parser.ConditionData.Count());
        }

        [Test]
        public void GreaterThanParser_ConstVar()
        {
            var parser = new ComparatorExpressionParser<Model>();

            StringAssert.Contains(parser.Parse(m => m.Age > 10), "Age > @Age0");
            Assert.AreEqual(1, parser.ConditionData.Count());
        }

        [Test]
        public void GreaterThanOrEqualParser_ConstVar()
        {
            var parser = new ComparatorExpressionParser<Model>();

            StringAssert.Contains(parser.Parse(m => m.Age >= 10), "Age >= @Age0");
            Assert.AreEqual(1, parser.ConditionData.Count());
        }

        [Test]
        public void SmallerThanOrEqualParser_ConstVar()
        {
            var parser = new ComparatorExpressionParser<Model>();

            StringAssert.Contains(parser.Parse(m => m.Age <= 10), "Age <= @Age0");
            Assert.AreEqual(1, parser.ConditionData.Count());
        }

        [Test]
        public void SmallerThanParser_ConstVar()
        {
            var parser = new ComparatorExpressionParser<Model>();

            StringAssert.Contains(parser.Parse(m => m.Age < 10), "Age < @Age0");
            Assert.AreEqual(1, parser.ConditionData.Count());
        }
        
        [Test]
        public void FieldParser()
        {
            var parser = new FieldGetterExpressionParser<Model, String>();
            Assert.AreEqual("Name", parser.Parse(m => m.Name));
        }
    }
}
