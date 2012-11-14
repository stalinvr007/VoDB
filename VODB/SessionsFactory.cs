using VODB.DbLayer;
using VODB.Sessions;

namespace VODB
{
    public static class SessionsFactory
    {
        public static ISession CreateEager(IDbConnectionCreator creator = null)
        {
            return new EagerInternalSession(creator);
        }

        public static ISession CreateStayAliveEager(IDbConnectionCreator creator = null)
        {
            return new StayAliveEagerInternalSession(creator);
        }
    }
}