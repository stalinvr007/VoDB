using System.Data.Common;
using VODB.EntityValidators;
using VODB.Extensions;

namespace VODB.DbLayer.DbCommands
{
    internal sealed class DbEntityUpdateCommandFactory<TEntity> : DbEntityCommandFactory<TEntity> where TEntity : DbEntity, new()
    {
        public DbEntityUpdateCommandFactory(IInternalSession internalSession, TEntity entity)
            : base(internalSession, entity)
        {
        }

        protected override DbCommand Make(DbCommand dbCommand, TEntity entity)
        {
            entity.ValidateEntity(On.Update);

            dbCommand.CommandText = entity.Table.CommandsHolder.Update;
            dbCommand.SetParameters(entity.Table.Fields, entity);
            dbCommand.SetOldParameters(entity);

            return dbCommand;
        }
    }
}
