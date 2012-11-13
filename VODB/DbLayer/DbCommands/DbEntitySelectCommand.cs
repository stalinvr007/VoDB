using System.Data.Common;

namespace VODB.DbLayer.DbCommands
{
    internal sealed class DbEntitySelectCommand<TEntity> : DbEntityCommand<TEntity> where TEntity : DbEntity, new()
    {
        public DbEntitySelectCommand(DbConnection connection, TEntity entity)
            : base(connection, entity)
        {
        }

        protected override DbCommand Make(DbCommand dbCommand, TEntity entity)
        {
            dbCommand.CommandText = entity.Table.CommandsHolder.Select;
            return dbCommand;
        }
    }
}