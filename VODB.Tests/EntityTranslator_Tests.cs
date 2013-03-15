using NUnit.Framework;
using System;
using System.Collections.Generic;
using VODB.EntityTranslation;
using VODB.Tests.Models.Northwind;
using System.Linq;
using System.Diagnostics;
using VODB.Exceptions;
using System.Collections;
using VODB.Infrastructure;
using VODB.Annotations;

namespace VODB.Tests
{

    class Pair<TEntity>
    {
        private readonly String _Value;
        public Pair(String value)
        {
            _Value = value;
        }

        public KeyValuePair<Type, String> ToKVP()
        {
            return new KeyValuePair<Type, String>(typeof(TEntity), _Value);
        }

    }

    [TestFixture(typeof(EntityTranslator))]
    public class EntityTranslator_Tests<TEntityTranslator> where TEntityTranslator : IEntityTranslator, new()
    {

        IEntityTranslator translator;
        
        public EntityTranslator_Tests()
        {
            translator = new TEntityTranslator();
        }

        private IEnumerable GetTablesNames()
        {
            return Utils.TestModels
                  .ToTables(translator)
                  .Select(t => new TestCaseData(t));
        }

        [TestCaseSource("GetTablesNames")]
        public void Translate_Assert_TableName(ITable table)
        {
            var attr = table.EntityType.GetCustomAttributes(typeof(DbTableAttribute), true).Cast<DbTableAttribute>().FirstOrDefault();

            if (attr != null)
            {
                Assert.That(table.Name, Is.EqualTo(attr.TableName));
                Assert.That(table.Name, Is.Not.Null);
                Assert.That(table.Name.Length, Is.GreaterThan(1));
            }
            else
            {
                Assert.That(table.Name, Is.EqualTo(table.EntityType.Name));
            }
        }
                
        [Test]
        public void Translate_Assert_FieldCount()
        {
            var table = translator.Translate(typeof(Employee));

            Assert.That(table.Fields.Count(), Is.EqualTo(18));
        }

        [TestCaseSource("GetTablesNames")]
        public void Translate_Assert_Fields(ITable table)
        {
            var entity = Activator.CreateInstance(table.EntityType);

            Assert.That(table.Fields.Any());
            Assert.That(table.Keys.Any());

            CollectionAssert.AllItemsAreNotNull(table.Fields);
            CollectionAssert.AllItemsAreNotNull(table.Keys);

            CollectionAssert.AllItemsAreUnique(table.Fields);
            CollectionAssert.AllItemsAreUnique(table.Keys);

            CollectionAssert.AllItemsAreNotNull(table.Fields.Select(i => i.Name));
            CollectionAssert.AllItemsAreNotNull(table.Keys.Select(i => i.Name));

            CollectionAssert.IsNotEmpty(table.Fields.Select(f => f.GetFieldFinalValue(entity)));
            CollectionAssert.IsNotEmpty(table.Keys.Select(f => f.GetFieldFinalValue(entity)));

        }

        [Test]
        public void Translate_Assert_Getter()
        {
            var table = translator.Translate(typeof(Employee));

            var employee = new Employee
            {
                EmployeeId = 1,
                BirthDate = new DateTime(1983, 4, 16)
            };

            var employeeIdField = table.Fields.First(f => f.Name.Equals("EmployeeId"));

            Assert.That(employeeIdField.GetValue(employee), Is.EqualTo(1));

            Assert.That(employeeIdField.GetValue(employee), Is.EqualTo(employeeIdField.GetFieldFinalValue(employee)));
        }

        [Test]
        [ExpectedException(typeof(UnableToGetTheFieldValueException))]
        public void Translate_Assert_Getter_WrongEntity()
        {
            var table = translator.Translate(typeof(Employee));

            var employee = new Orders // wrong type
            {
                OrderId = 1
            };

            var employeeIdField = table.Fields.First(f => f.Name.Equals("EmployeeId"));

            Assert.That(employeeIdField.GetValue(employee), Is.EqualTo(1));
        }

        [Test]
        public void Translate_Assert_Setter()
        {
            var table = translator.Translate(typeof(Employee));

            var employee = new Employee
            {
                EmployeeId = -1,
                BirthDate = new DateTime(1983, 4, 16)
            };

            var employeeIdField = table.Fields.First(f => f.Name.Equals("EmployeeId"));

            employeeIdField.SetValue(employee, 1);

            Assert.That(employee.EmployeeId, Is.EqualTo(1));
        }

        [TestCase("1")]
        [TestCase(true)]
        [TestCase(null)]
        [ExpectedException(typeof(UnableToSetTheFieldValueException))]
        public void Translate_Assert_Setter_WrongType(Object value)
        {
            var table = translator.Translate(typeof(Employee));

            var employee = new Employee
            {
                EmployeeId = -1,
                BirthDate = new DateTime(1983, 4, 16)
            };

            var employeeIdField = table.Fields.First(f => f.Name.Equals("EmployeeId"));

            employeeIdField.SetValue(employee, value);
        }

        [Test]
        public void Translate_Assert_BindedFieldValue()
        {
            var table = translator.Translate(typeof(Employee));

            var employee = new Employee
            {
                EmployeeId = -1,
                BirthDate = new DateTime(1983, 4, 16),
                ReportsTo = new Employee { EmployeeId = 10 }
            };

            var reportsToField = table.Fields.First(f => f.Name.Equals("ReportsTo"));

            Assert.That(reportsToField.GetFieldFinalValue(employee), Is.EqualTo(10));
        }

        private static IEnumerable GetTestValues()
        {
            yield return new TestCaseData("EmployeeId", 1);
            yield return new TestCaseData("FirstName", "Sérgio");
            yield return new TestCaseData("LastName", "Ferreira");
            yield return new TestCaseData("Title", "Eng");
            yield return new TestCaseData("TitleOfCourtesy", "Eng");
            yield return new TestCaseData("BirthDate", new DateTime(1983, 4, 16));
            yield return new TestCaseData("HireDate", new DateTime(2001, 4, 16));
            yield return new TestCaseData("Address", "av. bla bla number x");
            yield return new TestCaseData("City", "Lisbon");
            yield return new TestCaseData("Region", "West Coast");
            yield return new TestCaseData("PostalCode", "1170");
            yield return new TestCaseData("Country", "Portugal");
            yield return new TestCaseData("HomePhone", "964585858");
            yield return new TestCaseData("Extension", "2514");
            yield return new TestCaseData("Notes", "some notes");
            yield return new TestCaseData("Photo", new Byte[0]);
            yield return new TestCaseData("ReportsTo", new Employee { EmployeeId = 10 });
            yield return new TestCaseData("PhotoPath", "/");
        }

        [TestCaseSource("GetTestValues")]
        public void Translate_Assert_SetValue_GetValue(String fieldName, Object value)
        {
            var table = translator.Translate(typeof(Employee));

            var field = table.Fields.First(f => f.Name.Equals(fieldName));

            var employee = new Employee();

            field.SetValue(employee, value);

            Assert.That(field.GetValue(employee), Is.EqualTo(value));
        }

        [Test]
        public void Translate_Assert_SetAllValuesToEmployee()
        {
            var table = translator.Translate(typeof(Employee));

            var dic = new Dictionary<String, Object>()
            {
                {"EmployeeId", 1},
                {"LastName", "Ferreira"},
                {"FirstName", "Sérgio"},
                {"Title", "Eng"},
                {"TitleOfCourtesy", "Eng"},
                {"BirthDate", new DateTime(1983,4,16)},
                {"HireDate", DateTime.Now},
                {"Address", "av. bla bla number x"},
                {"City", "Lisbon"},
                {"Region", "West Coast"},
                {"PostalCode", "1170"},
                {"Country", "Portugal"},
                {"HomePhone", "964585858"},
                {"Extension", "2514"},
                {"Notes", "some notes"},
                {"Photo", new Byte[0]},
                {"ReportsTo", new Employee { EmployeeId = 10 }},
                {"PhotoPath", "/"}
            };

            var employee = new Employee();


            var fields = table.Fields.ToArray();

            var values = dic.Select(kvp => kvp.Value).ToArray();

            for (int i = 0; i < values.Length; i++)
            {
                fields[i].SetValue(employee, values[i]);
            }

            foreach (var field in fields)
            {
                Assert.That(field.GetValue(employee), Is.EqualTo(dic[field.Name]));
            }

        }

    }
}
