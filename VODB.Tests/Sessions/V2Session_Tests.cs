using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VODB.Core.Loaders.Factories;
using VODB.DbLayer;
using VODB.EntityMapping;
using VODB.EntityTranslation;
using VODB.Sessions;
using VODB.Sessions.EntityFactories;

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
            return new V2Session(
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
                        Assert.That(session.Delete(entity), Is.True);
                        Assert.That(session.Delete(entity), Is.False);
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
                    Assert.That(session.Update(entity), Is.Not.Null);
                    Assert.That(session.Update(entity), Is.Not.Null);
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
                    Assert.That(session.Insert(entity), Is.Not.Null);
                    try
                    {
                        Assert.That(session.Insert(entity), Is.Not.Null);
                    }
                    catch (SqlException ex)
                    {
                        StringAssert.Contains("PRIMARY KEY", ex.Message);
                    }
                    
                });
            }
        }

    }
}
