using VODB.DbLayer;
using VODB.Sessions;

namespace VODB
{
    public static class SessionsFactory
    {
        public static ISession CreateEager(IDbConnectionCreator creator = null)
        {
            return new InternalEagerSession(creator);
        }
    }
}