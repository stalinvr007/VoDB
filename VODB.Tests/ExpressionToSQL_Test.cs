using System;
using NUnit.Framework;
using VODB.ExpressionParser;
using VODB.Tests.Models.Northwind;
using VODB.ExpressionsToSql;
using System.Linq;
using VODB.Core;
using System.Diagnostics;
using VODB.Expressions;

namespace VODB.Tests
{
    [TestFixture]
    public class ExpressionToSQL_Test
    {
        int argumentValue = 3;

        [Test]
        public void Expression_Decoder()
        {
            var parts = new ExpressionPart[] {
                new ExpressionPart { PropertyName = "EmployeeId", EntityType = typeof(Employee), EntityTable = Engine.GetTable<Employee>() },
                new ExpressionPart { PropertyName = "ReportsTo", EntityType = typeof(Employee), EntityTable = Engine.GetTable<Employee>() },
                new ExpressionPart { PropertyName = "Employee", EntityType = typeof(Orders), EntityTable = Engine.GetTable<Orders>() }
            };

            var exp = new ExpressionDecoder<Orders, Boolean>(o => o.Employee.ReportsTo.EmployeeId == 3);
            var decoded = exp.DecodeLeft().ToList();

            Assert.AreEqual(3, exp.DecodeRight());
            Assert.AreEqual(3, decoded.Count);

            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(parts[i].PropertyName, decoded[i].PropertyName);
                Assert.AreEqual(parts[i].EntityTable.TableName, decoded[i].EntityTable.TableName);
                Assert.AreEqual(parts[i].EntityType, decoded[i].EntityType);
            }

            decoded.Reduce();

            Assert.AreEqual(2, decoded.Count);

        }

        [Test]
        public void Expression_ReturnsObject_Decoder()
        {
            var parts = new ExpressionPart[] {
                new ExpressionPart { PropertyName = "ReportsTo", EntityType = typeof(Employee), EntityTable = Engine.GetTable<Employee>() },
                new ExpressionPart { PropertyName = "Employee", EntityType = typeof(Orders), EntityTable = Engine.GetTable<Orders>() }
            };

            var exp = new ExpressionDecoder<Orders, Object>(o => o.Employee.ReportsTo);
            var decoded = exp.DecodeLeft().ToList();
            Assert.AreEqual(2, decoded.Count);

            for (int i = 0; i < 2; i++)
            {
                Assert.AreEqual(parts[i].PropertyName, decoded[i].PropertyName);
                Assert.AreEqual(parts[i].EntityTable.TableName, decoded[i].EntityTable.TableName);
                Assert.AreEqual(parts[i].EntityType, decoded[i].EntityType);
            }
        }

        
        [Test]
        public void ExpressionToSQL_Simple_Query()
        {
            var query = new QueryCondition<Orders>(o => o.OrderId == argumentValue);

            Assert.AreEqual("OrderId = @p0", query.Compile(0));
            Assert.That(query.Parameters.Count(), Is.EqualTo(1));
            Assert.That(query.Parameters.First().Value, Is.EqualTo(3));
            Assert.That(query.Parameters.First().Name, Is.EqualTo("@p0"));
        }

        [Test]
        public void ExpressionToSql_Simple_NoCompare()
        {
            var query = new QueryCondition<Orders>(o => o.OrderId,
                new QueryCondition<Orders>(o => o.OrderId == 3));


        }

        [Test]
        public void ExpressionToSQL_Multiple_Levels()
        {
            var query = new QueryCondition<Orders>(o => o.Employee.ReportsTo.EmployeeId == 3);

            Assert.AreEqual("EmployeeId in (Select EmployeeId From Employees Where ReportsTo in (Select EmployeeId From Employees Where EmployeeId = @p0))", query.Compile(0));
            Assert.That(query.Parameters.Count(), Is.EqualTo(1));
            Assert.That(query.Parameters.First().Value, Is.EqualTo(3));
            Assert.That(query.Parameters.First().Name, Is.EqualTo("@p0"));
        }

        [Test]
        public void ExpressionToSQL_Multiple_Levels2()
        {
            var query = new QueryCondition<Orders>(o => o.Employee.ReportsTo.ReportsTo.EmployeeId == 3);

            string queryCompile = query.Compile(0);
            Assert.AreEqual("EmployeeId in (Select EmployeeId From Employees Where ReportsTo in (Select EmployeeId From Employees Where ReportsTo in (Select EmployeeId From Employees Where EmployeeId = @p0)))", queryCompile);
            Assert.That(query.Parameters.Count(), Is.EqualTo(1));
            Assert.That(query.Parameters.First().Value, Is.EqualTo(3));
            Assert.That(query.Parameters.First().Name, Is.EqualTo("@p0"));
        }


        [Test]
        public void ExpressionToSQL_Composite_Multiple_Levels()
        {
            var query = new QueryCondition();

            for (int i = 0; i < 1000; i++)
            {
                query.Add(new QueryCondition<Orders>(o => o.Employee.ReportsTo.EmployeeId == 3));
            }
            
            var compiledQuery = query.Compile(0);

            Assert.AreEqual(128893, compiledQuery.Count());
            Assert.That(query.Parameters.Count(), Is.EqualTo(1000));

            foreach (var parameter in query.Parameters)
            {
                Assert.That(parameter.Name, Is.StringStarting("@p"));
                Assert.That(parameter.Value, Is.EqualTo(3));
            }
        }

    }
}
