using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VODB.DbLayer;
using VODB.EntityMapping;
using VODB.EntityTranslation;
using VODB.Sessions;

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
            return new V2Session(new VodbConnection(Utils.ConnectionCreator), new EntityTranslator(), new OrderedEntityMapper());
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

    }
}
