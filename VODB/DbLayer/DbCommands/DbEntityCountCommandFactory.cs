using System.Data.Common;

namespace VODB.DbLayer.DbCommands
{
    internal sealed class DbEntityCountCommandFactory<TEntity> : DbEntityCommandFactory<TEntity> where TEntity : DbEntity, new()
    {
        public DbEntityCountCommandFactory(IInternalSession internalSession)
            : base(internalSession, new TEntity())
        { }

        protected override DbCommand Make(DbCommand dbCommand, TEntity entity)
        {
            dbCommand.CommandText = entity.Table.CommandsHolder.Count;
            return dbCommand;
        }
    }
}
