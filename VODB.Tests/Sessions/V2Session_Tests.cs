using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VODB.DbLayer;
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

        [TestCaseSource("GetEntities")]
        public void V2Session_Assert_Count<TEntity>(TEntity entity, int count) where TEntity : class, new()
        {
            var session = new V2Session(new VodbConnection(Utils.ConnectionCreator), new EntityTranslator());
            int entityCount = session.Count<TEntity>();
            Assert.That(entityCount, Is.EqualTo(count));
        }

    }
}
