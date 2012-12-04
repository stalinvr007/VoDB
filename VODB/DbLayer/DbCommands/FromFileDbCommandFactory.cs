using System.Data.Common;
using System;
using System.IO;

namespace VODB.DbLayer.DbCommands
{
    internal class FromFileTSQLDbCommandFactory : DbCommandFactory
    {
        private readonly String _FilePath;
        protected FromFileTSQLDbCommandFactory(IInternalSession internalSession, String filePath)
            : base(internalSession)
        {
            _FilePath = filePath;

        }

        protected override DbCommand Make(DbCommand dbCommand)
        {
            dbCommand.CommandText = File.ReadAllText(_FilePath);
            return dbCommand;
        }
    }
}
