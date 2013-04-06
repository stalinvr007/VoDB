using VODB.DbLayer;
using VODB.EntityMapping;
using VODB.EntityTranslation;
using VODB.Sessions;
using VODB.Sessions.EntityFactories;

namespace VODB
{
    public class Session : SessionBase
    {
        public Session() : base(GetSession(new NameConventionDbConnectionCreator("System.Data.SqlClient")))
        { }

        public Session(IDbConnectionCreator connectionCreator) : base(GetSession(connectionCreator))
        { }

        private static ISession GetSession(IDbConnectionCreator connectionCreator)
        {
            return new SessionV2(
                new VodbConnection(connectionCreator),
                new EntityTranslator(),
                new OrderedEntityMapper(),
                new ProxyCreator());
        }
    }
}
