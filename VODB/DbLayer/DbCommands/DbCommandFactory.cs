using System.Data.Common;

namespace VODB.DbLayer.DbCommands
{
    internal abstract class DbCommandFactory : IDbCommandFactory
    {

        private readonly ISessionInternal _session;

        protected DbCommandFactory(ISessionInternal session)
        {
            _session = session;
        }

        public DbCommand Make()
        {
            return Make(_session.CreateCommand());
        }

        protected abstract DbCommand Make(DbCommand dbCommand);
    }
}