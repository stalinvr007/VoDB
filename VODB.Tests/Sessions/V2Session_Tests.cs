using NUnit.Framework;
using System;
using System.Collections;
using System.Data.SqlClient;
using System.Linq;
using VODB.DbLayer;
using VODB.EntityMapping;
using VODB.EntityTranslation;
using VODB.Sessions;
using VODB.Sessions.EntityFactories;
using VODB.Tests.Models.Northwind;

namespace VODB.Tests.Sessions
{
    [TestFixture]
    public class V2Session_Tests
    {

        private IEnumerable GetEntities()
        {
            return Utils.RecordCounts
                .Select(kvp => new TestCaseData(Activator.CreateInstance(kvp.Key), kvp.Value));
        }

        private static ISession GetSession()
        {
            return new SessionV2(
                new VodbConnection(Utils.ConnectionCreator), 
                new EntityTranslator(), 
                new OrderedEntityMapper(), 
                new ProxyCreator()
            );
        }

        [TestCaseSource("GetEntities")]
        public void V2Session_Assert_Count<TEntity>(TEntity entity, int count) where TEntity : class, new()
        {
            using (var session = GetSession())
            {
                Assert.That(session.Count<TEntity>(), Is.EqualTo(count));
                Assert.That(session.Count<TEntity>(), Is.EqualTo(count));
            }
        }

        private IEnumerable GetEntitiesById()
        {
            return Utils.TestModels.ToTables()
                .Where(t => t.Name != "CustomerDemographics")
                .Where(t => t.Name != "CustomerCustomerDemo")
                .Select(t => t.CreateExistingTestEntity());
        }

        [TestCaseSource("GetEntitiesById")]
        public void V2Session_Assert_GetById<TEntity>(TEntity entity) where TEntity : class, new()
        {
            using (var session = GetSession())
            {
                Assert.That(session.GetById(entity), Is.Not.Null);
                Assert.That(session.GetById(entity), Is.Not.Null);
            }
        }

        [TestCaseSource("GetEntitiesById")]
        public void V2Session_Assert_Exists<TEntity>(TEntity entity) where TEntity : class, new()
        {
            using (var session = GetSession())
            {
                Assert.That(session.Exists(entity), Is.True);
                Assert.That(session.Exists(entity), Is.True);
            }
        }

        [TestCaseSource("GetEntitiesById")]
        public void V2Session_Assert_Delete<TEntity>(TEntity entity) where TEntity : class, new()
        {
            using (var session = GetSession())
            {
                session.WithRollback(s =>
                {
                    try
                    {
                        Assert.That(s.Delete(entity), Is.True);
                        Assert.That(s.Delete(entity), Is.False);
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("REFERENCE constraint "))
                        {
                            return;
                        }
                        throw;
                    }
                    
                });
            }
        }

        [TestCaseSource("GetEntitiesById")]
        public void V2Session_Assert_Update<TEntity>(TEntity entity) where TEntity : class, new()
        {
            using (var session = GetSession())
            {
                session.WithRollback(s =>
                {
                    Assert.That(s.Update(entity), Is.Not.Null);
                    Assert.That(s.Update(entity), Is.Not.Null);
                });
            }
        }

        private IEnumerable GetUnExistingEntities()
        {
            return Utils.TestModels.ToTables()
                .Where(t => t.Name != "CustomerDemographics")
                .Where(t => t.Name != "CustomerCustomerDemo")
                .Select(t => t.CreateUnExistingTestEntity());
        }

        [TestCaseSource("GetUnExistingEntities")]
        public void V2Session_Assert_Insert<TEntity>(TEntity entity) where TEntity : class, new()
        {
            using (var session = GetSession())
            {
                session.WithRollback(s =>
                {
                    Assert.That(s.Insert(entity), Is.Not.Null);
                    try
                    {
                        Assert.That(s.Insert(entity), Is.Not.Null);
                    }
                    catch (SqlException ex)
                    {
                        StringAssert.Contains("PRIMARY KEY", ex.Message);
                    }
                    
                });
            }
        }

        private IEnumerable GetPrecompiledQueries()
        {
            yield return new TestCaseData(
                Select.All.From<Employee>().Where(f => f.EmployeeId > Param.Get<int>()),
                new Object[] { 1 }, /* Query arguments */
                8
            );

            yield return new TestCaseData(
                Select.All.From<Employee>().Where(f => f.ReportsTo == Param.Get<Employee>()),
                new Object[] { new Employee{ EmployeeId = 2 } }, /* Query arguments */
                1
            );
        }

        [TestCaseSource("GetPrecompiledQueries")]
        public  void V2Session_Assert_PrecompiledQuery(IQuery<Employee> query, Object[] queryArgs, int recordCount)
        {
            using (var session = GetSession())
            {
                for (int i = 0; i < 3; i++) /* Execute this query more than once. */
                {
                    var employees = session.ExecuteQuery(query, queryArgs).ToList();

                    Assert.That(employees.Count(), Is.EqualTo(recordCount));
                    CollectionAssert.IsNotEmpty(employees);
                    CollectionAssert.AllItemsAreNotNull(employees);
                    CollectionAssert.AllItemsAreUnique(employees);

                    foreach (var employee in employees)
                    {
                        Assert.That(employee.EmployeeId, Is.GreaterThan(0));
                        Assert.That(employee.LastName, Is.Not.Null);
                        Assert.That(employee.LastName, Is.Not.Empty);

                        Assert.That(employee.FirstName, Is.Not.Null);
                        Assert.That(employee.FirstName, Is.Not.Empty);
                    }    
                }
            }
        }
    }
}
