using System;
using System.Data.Common;

namespace VODB.DbLayer.DbCommands
{
    internal sealed class DbCommandBypass : DbCommandFactory
    {
        private readonly string _cmdMask;
        private readonly object[] _args;

        public DbCommandBypass(IInternalSession internalSession, String cmdMask, params Object[] args)
            : base(internalSession)
        {
            _cmdMask = cmdMask;
            _args = args;
        }

        protected override DbCommand Make(DbCommand dbCommand)
        {
            dbCommand.CommandText = String.Format(_cmdMask, _args);
            return dbCommand;
        }
    }
}
