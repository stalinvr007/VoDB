using System;
using NUnit.Framework;
using VODB.ExpressionParser;
using VODB.Tests.Models.Northwind;
using VODB.ExpressionsToSql;
using System.Linq;
using VODB.Core;
using System.Diagnostics;

namespace VODB.Tests
{
    [TestFixture]
    public class ExpressionToSQL_Test
    {
        [Test]
        public void ExpressionToSql_Decoder()
        {
            var parts = new ExpressionPart[] {
                new ExpressionPart { PropertyName = "EmployeeId", EntityType = typeof(Employee), EntityTable = Engine.GetTable<Employee>() },
                new ExpressionPart { PropertyName = "ReportsTo", EntityType = typeof(Employee), EntityTable = Engine.GetTable<Employee>() },
                new ExpressionPart { PropertyName = "Employee", EntityType = typeof(Orders), EntityTable = Engine.GetTable<Orders>() }
            };

            var exp = new ExpressionDecoder<Orders>(o => o.Employee.ReportsTo.EmployeeId == 3);
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
        public void ExpressionToSQL_Simple_Query()
        {
            var query = new QueryCondition<Orders>(o => o.OrderId == 3);

            Assert.AreEqual("OrderId = @p0", query.Compile(0));
            Assert.That(query.Parameters.Count(), Is.EqualTo(1));
            Assert.That(query.Parameters.First().Value, Is.EqualTo(3));
            Assert.That(query.Parameters.First().Name, Is.EqualTo("@p0"));
        }

        public void ExpressionToSql_Simple_NoCompare()
        {
            var query = new QueryLeft<Orders, int>(o => o.OrderId);
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
