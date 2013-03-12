using NUnit.Framework;
using System;
using System.Collections.Generic;
using VODB.EntityTranslation;
using VODB.Tests.Models.Northwind;
using System.Linq;

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
            var translator = new EntityTranslator();
            foreach (var pair in EntityTables.AsParallel())
            {
                Assert.That(translator.Translate(pair.Key).Name, Is.EqualTo(pair.Value));
            }

        }

    }
}
