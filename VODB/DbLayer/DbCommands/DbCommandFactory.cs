using System.Data.Common;

namespace VODB.DbLayer.DbCommands
{
    internal abstract class DbCommandFactory : IDbCommandFactory
    {

        private readonly IInternalSession _internalSession;

        protected DbCommandFactory(IInternalSession internalSession)
        {
            _internalSession = internalSession;
        }

        public DbCommand Make()
        {
            return Make(_internalSession.CreateCommand());
        }

        protected abstract DbCommand Make(DbCommand dbCommand);
    }
}