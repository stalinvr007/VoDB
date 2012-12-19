using System.Data.Common;
using VODB.EntityValidators;
using VODB.Extensions;

namespace VODB.DbLayer.DbCommands
{
    internal sealed class DbEntityDeleteCommandFactory<TEntity> : DbEntityCommandFactory<TEntity> 
        where TEntity : new()
    {
        public DbEntityDeleteCommandFactory(IInternalSession internalSession, TEntity entity)
            : base(internalSession, entity)
        {
        }

        protected override DbCommand Make(DbCommand dbCommand, TEntity entity)
        {
            var inEntity = entity as Entity;
            inEntity.ValidateEntity(On.Delete);

            dbCommand.CommandText = inEntity.Table.CommandsHolder.Delete;
            dbCommand.SetParameters(inEntity.Table.KeyFields, inEntity);
            return dbCommand;
        }
    }
}
