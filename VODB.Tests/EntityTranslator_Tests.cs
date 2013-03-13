using NUnit.Framework;
using System;
using System.Collections.Generic;
using VODB.EntityTranslation;
using VODB.Tests.Models.Northwind;
using System.Linq;
using System.Diagnostics;
using VODB.Exceptions;

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

    [TestFixture] 
    public class EntityTranslator_Tests
    {
        IEntityTranslator translator = new EntityTranslator();
        IList<KeyValuePair<Type, String>> EntityTables = new List<KeyValuePair<Type, String>>
        {
            new Pair<Categories>("Categories").ToKVP(),
            new Pair<CustomerCustomerDemo>("CustomerCustomerDemo").ToKVP(),
            new Pair<CustomerDemographics>("CustomerDemographics").ToKVP(),
            new Pair<Customers>("Customers").ToKVP(),
            new Pair<Employee>("Employees").ToKVP(),
            new Pair<EmployeeTerritories>("EmployeeTerritories").ToKVP(),
            new Pair<OrderDetails>("Order Details").ToKVP(),
            new Pair<Orders>("Orders").ToKVP(),
            new Pair<Products>("Products").ToKVP(),
            new Pair<Region>("Region").ToKVP(),
            new Pair<Shippers>("Shippers").ToKVP(),
            new Pair<Suppliers>("Suppliers").ToKVP(),
            new Pair<Territories>("Territories").ToKVP()
        };

        [Test]
        public void Translate_Assert_TableNames()
        {
            foreach (var pair in EntityTables.AsParallel())
            {
                var table = translator.Translate(pair.Key);
                Assert.That(table.Name, Is.EqualTo(pair.Value));
            }
        }

        [Test]
        public void Translate_Assert_Fields()
        {
            foreach (var pair in EntityTables.AsParallel())
            {
                var table = translator.Translate(pair.Key);

                Assert.That(table.Fields.Any());
                Assert.That(table.Keys.Any());

                CollectionAssert.AllItemsAreNotNull(table.Fields);
                CollectionAssert.AllItemsAreNotNull(table.Keys);

                CollectionAssert.AllItemsAreUnique(table.Fields);
                CollectionAssert.AllItemsAreUnique(table.Keys);

                CollectionAssert.AllItemsAreNotNull(table.Fields.Select(i => i.Name));
                CollectionAssert.AllItemsAreNotNull(table.Keys.Select(i => i.Name));
            }
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
        }

        [Test]
        [ExpectedException(typeof(UnableToGetTheFieldValueException))]
        public void Translate_Assert_Getter_WrongEntity()
        {
            var table = translator.Translate(typeof(Employee));

            var employee = new Orders
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
                
    }
}
