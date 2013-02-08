using System.Data.Common;

namespace VODB.Core.Execution.Factories
{
    internal abstract class DbCommandFactory : IDbCommandFactory
    {
        private readonly IInternalSession _internalSession;

        protected DbCommandFactory(IInternalSession internalSession)
        {
            _internalSession = internalSession;
        }

        #region IDbCommandFactory Members

        public DbCommand Make()
        {
            return Make(_internalSession.CreateCommand());
        }

        #endregion

        protected abstract DbCommand Make(DbCommand dbCommand);
    }
}