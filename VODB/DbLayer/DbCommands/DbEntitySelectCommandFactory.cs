using System.Data.Common;

namespace VODB.DbLayer.DbCommands
{
    internal sealed class DbEntitySelectCommandFactory<TEntity> : DbEntityCommandFactory<TEntity> 
        where TEntity : DbEntity, new()
    {
        public DbEntitySelectCommandFactory(IInternalSession internalSession)
            : base(internalSession, new TEntity())
        {
        }

        protected override DbCommand Make(DbCommand dbCommand, TEntity entity)
        {
            dbCommand.CommandText = entity.Table.CommandsHolder.Select;
            return dbCommand;
        }
    }
}