﻿using System;
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

            Assert.AreEqual(3, exp.DecodeRight().First());
            Assert.AreEqual(3, decoded.Count);

            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(parts[i].PropertyName, decoded[i].PropertyName);
                Assert.AreEqual(parts[i].EntityTable.TableName, decoded[i].EntityTable.TableName);
                Assert.AreEqual(parts[i].EntityType, decoded[i].EntityType);
            }
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
        public void Expression_ReturnsProperty_Decoder()
        {
            var parts = new ExpressionPart[] {
                new ExpressionPart { PropertyName = "EmployeeId", EntityType = typeof(Employee), EntityTable = Engine.GetTable<Employee>() },
                new ExpressionPart { PropertyName = "Employee", EntityType = typeof(Orders), EntityTable = Engine.GetTable<Orders>() }
            };

            var exp = new ExpressionDecoder<Orders, Object>(o => o.Employee.EmployeeId);
            var decoded = exp.DecodeLeft().ToList();
            Assert.AreEqual(2, decoded.Count);

            for (int i = 0; i < 2; i++)
            {
                Assert.AreEqual(parts[i].PropertyName, decoded[i].PropertyName);
                Assert.AreEqual(parts[i].EntityTable.TableName, decoded[i].EntityTable.TableName);
                Assert.AreEqual(parts[i].EntityType, decoded[i].EntityType);
            }
        }

        private static void AssertQuery(ref int level, QueryCondition<Orders> query, String expected, String parameter)
        {
            Assert.AreEqual(expected, query.Compile(ref level));
            Assert.That(query.Parameters.Count(), Is.EqualTo(1));
            Assert.That(query.Parameters.First().Value, Is.EqualTo(3));
            Assert.That(query.Parameters.First().Name, Is.EqualTo(parameter));
        }

        [Test]
        public void ExpressionToSQL_Simple_Query()
        {
            var level = 0;
            AssertQuery(ref level, new QueryCondition<Orders>(o => o.OrderId == argumentValue), "OrderId = @p1", "@p1");
            AssertQuery(ref level, new QueryCondition<Orders>(o => o.OrderId < argumentValue), "OrderId < @p2", "@p2");
            AssertQuery(ref level, new QueryCondition<Orders>(o => o.OrderId <= argumentValue), "OrderId <= @p3", "@p3");
            AssertQuery(ref level, new QueryCondition<Orders>(o => o.OrderId > argumentValue), "OrderId > @p4", "@p4");
            AssertQuery(ref level, new QueryCondition<Orders>(o => o.OrderId >= argumentValue), "OrderId >= @p5", "@p5");
        }

        [Test]
        public void ExpressionToSQL_StubCondition()
        {
            var query = new QueryCondition<Orders>(o => o.OrderId, new StubCondition());
            var level = 0;
            Assert.That(query.Compile(ref level), Is.EqualTo("OrderId"));
        }

        [Test]
        public void ExpressionToSQL_OrderByCondition()
        {
            var query = new OrderByCondition<Orders>(o => o.OrderId);
            var level = 0;
            Assert.That(query.Compile(ref level), Is.EqualTo(" Order By OrderId"));
        }

        [Test]
        public void ExpressionToSql_InCondition()
        {
            var level = 0;
            var query = new QueryCondition<Orders>(o => o.OrderId,
                new InCondition(new Object[] { 1, 2, 3, 4 }));

            Assert.That(query.Compile(ref level), Is.EqualTo("OrderId In (@p1, @p2, @p3, @p4)"));
            Assert.That(query.Parameters.Count(), Is.EqualTo(4));

            var i = 0;
            foreach (var parameter in query.Parameters)
            {
                Assert.That(parameter.Name, Is.EqualTo("@p" + ++i));
            }
        }

        [Test]
        public void ExpressionToSql_Between()
        {
            var level = 0;
            var query = new QueryCondition<Orders>(o => o.OrderId, new BetweenCondition(1, 3));

            Assert.That(query.Compile(ref level), Is.EqualTo("OrderId Between @p1 and @p2"));
            Assert.That(query.Parameters.Count(), Is.EqualTo(2));

            var i = 0;
            foreach (var parameter in query.Parameters)
            {
                Assert.That(parameter.Name, Is.EqualTo("@p" + ++i));
            }

        }

        [Test]
        public void ExpressionToSQL_Multiple_Levels()
        {
            var level = 0;
            var query = new QueryCondition<Orders>(o => o.Employee.ReportsTo.EmployeeId == 3);

            Assert.AreEqual("EmployeeId in (Select EmployeeId From Employees Where ReportsTo in (Select EmployeeId From Employees Where EmployeeId = @p1))", query.Compile(ref level));
            Assert.That(query.Parameters.Count(), Is.EqualTo(1));
            Assert.That(query.Parameters.First().Value, Is.EqualTo(3));
            Assert.That(query.Parameters.First().Name, Is.EqualTo("@p1"));
        }

        [Test]
        public void ExpressionToSQL_Multiple_Levels2()
        {
            var level = 0;
            var query = new QueryCondition<Orders>(o => o.Employee.ReportsTo.ReportsTo.EmployeeId == 3);

            string queryCompile = query.Compile(ref level);
            Assert.AreEqual("EmployeeId in (Select EmployeeId From Employees Where ReportsTo in (Select EmployeeId From Employees Where ReportsTo in (Select EmployeeId From Employees Where EmployeeId = @p1)))", queryCompile);
            Assert.That(query.Parameters.Count(), Is.EqualTo(1));
            Assert.That(query.Parameters.First().Value, Is.EqualTo(3));
            Assert.That(query.Parameters.First().Name, Is.EqualTo("@p1"));
        }


        [Test]
        public void ExpressionToSQL_Composite_Multiple_Levels()
        {
            var level = 0;
            var query = new QueryCondition();

            for (int i = 0; i < 1000; i++)
            {
                query.Add(new QueryCondition<Orders>(o => o.Employee.ReportsTo.EmployeeId == 3));
            }

            var compiledQuery = query.Compile(ref level);

            Assert.AreEqual(126893, compiledQuery.Count());
            Assert.That(query.Parameters.Count(), Is.EqualTo(1000));

            foreach (var parameter in query.Parameters)
            {
                Assert.That(parameter.Name, Is.StringStarting("@p"));
                Assert.That(parameter.Value, Is.EqualTo(3));
            }
        }

    }
}
