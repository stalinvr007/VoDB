using VODB.DbLayer;

namespace VODB
{
    public static class Sessions
    {
        public static ISession GetEager(IDbConnectionCreator creator = null)
        {
            return new EagerSession(creator);
        }
    }
}