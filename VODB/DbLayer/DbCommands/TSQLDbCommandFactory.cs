using System.Data.Common;
using System;

namespace VODB.DbLayer.DbCommands
{
    internal class TSQLDbCommandFactory : DbCommandFactory
    {
        private readonly String _TextCommand;

        public TSQLDbCommandFactory(IInternalSession internalSession, String textCommand)
            : base(internalSession)
        {
            _TextCommand = textCommand;
        }

        protected override DbCommand Make(DbCommand dbCommand)
        {
            dbCommand.CommandText = _TextCommand;
            return dbCommand;
        }
    }
}
