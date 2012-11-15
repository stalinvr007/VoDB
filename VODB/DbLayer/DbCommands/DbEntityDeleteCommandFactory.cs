using System.Data.Common;
using VODB.EntityValidators;
using VODB.Extensions;

namespace VODB.DbLayer.DbCommands
{
    internal sealed class DbEntityDeleteCommandFactory<TEntity> : DbEntityCommandFactory<TEntity> where TEntity : DbEntity, new()
    {
        public DbEntityDeleteCommandFactory(IInternalSession internalSession, TEntity entity)
            : base(internalSession, entity)
        {
        }

        protected override DbCommand Make(DbCommand dbCommand, TEntity entity)
        {
            entity.ValidateEntity(On.Delete);

            dbCommand.CommandText = entity.Table.CommandsHolder.Delete;
            dbCommand.SetParameters(entity.Table.KeyFields, entity);
            return dbCommand;
        }
    }
}
