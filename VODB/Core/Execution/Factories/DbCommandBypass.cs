using System;
using System.Data;
using System.Data.Common;

namespace VODB.Core.Execution.Factories
{
    internal sealed class DbCommandBypass : DbCommandFactory
    {
        private readonly object[] _args;
        private readonly string _cmdMask;

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