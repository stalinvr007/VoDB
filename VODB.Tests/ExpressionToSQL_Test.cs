using System;
using NUnit.Framework;
using VODB.ExpressionParser;
using VODB.Tests.Models.Northwind;
using VODB.ExpressionsToSql;
using System.Linq;
using VODB.Core;

namespace VODB.Tests
{
    [TestFixture]
    public class ExpressionToSQL_Test
    {
        [Test]
        public void ExpressionToSql_Decoder()
        {
            var exp = new ExpressionDecoder<Orders>(o => o.Employee.ReportsTo.EmployeeId == 3);

            var decoded = exp.DecodeLeft().ToArray();
            Assert.AreEqual(3, decoded.Length);

            var parts = new ExpressionPart[] {
                new ExpressionPart { PropertyName = "EmployeeId", EntityType = typeof(Employee), EntityTable = Engine.GetTable<Employee>() },
                new ExpressionPart { PropertyName = "ReportsTo", EntityType = typeof(Employee), EntityTable = Engine.GetTable<Employee>() },
                new ExpressionPart { PropertyName = "Employee", EntityType = typeof(Orders), EntityTable = Engine.GetTable<Orders>() }
            };

            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(parts[i].PropertyName, decoded[i].PropertyName);
                Assert.AreEqual(parts[i].EntityTable.TableName, decoded[i].EntityTable.TableName);
                Assert.AreEqual(parts[i].EntityType, decoded[i].EntityType);
            }
        }

        [Test]
        public void ExpressionToSQL_Simple_Query()
        {
            var query = new Query<Orders>(o => o.OrderId == 3);

            Assert.AreEqual("Select * From Orders Where OrderId = 3", query.Compile());
        }

        [Test]
        public void ExpressionToSQL_Multiple_Levels()
        {
            var query = new Query<Orders>(o => o.Employee.ReportsTo.EmployeeId == 3);

            Assert.AreEqual("", query.Compile());
        }
    }
}
