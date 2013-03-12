using NUnit.Framework;
using System;
using System.Collections.Generic;
using VODB.EntityTranslation;
using VODB.Tests.Models.Northwind;
using System.Linq;

namespace VODB.Tests
{
    [TestFixture]
    public class EntityTranslator_Tests
    {

        IList<KeyValuePair<Type, String>> EntityTables = new List<KeyValuePair<Type, String>>
        {
            new KeyValuePair<Type, String>(typeof(Employee), "Employees")
        };

        [Test]
        public void Translate_Assert_TableNames()
        {
            var translator = new EntityTranslator();
            foreach (var pair in EntityTables.AsParallel())
            {
                Assert.That(translator.Translate(pair.Key).Name, Is.EqualTo(pair.Value));
            }

        }

    }
}
