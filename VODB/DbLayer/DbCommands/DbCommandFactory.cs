using System.Data.Common;

namespace VODB.DbLayer.DbCommands
{
    internal abstract class DbCommandFactory : IDbCommandFactory
    {

        private readonly DbConnection _connection;

        protected DbCommandFactory(DbConnection connection)
        {
            _connection = connection;
        }

        public DbCommand Make()
        {
            return Make(_connection.CreateCommand());
        }

        protected abstract DbCommand Make(DbCommand dbCommand);
    }
}