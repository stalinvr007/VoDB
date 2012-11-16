using System.Data.Common;
using VODB.EntityValidators;
using VODB.Extensions;

namespace VODB.DbLayer.DbCommands
{
    internal sealed class DbEntityCountByIdCommandFactory<TEntity> : DbEntityCommandFactory<TEntity>
            where TEntity : DbEntity, new()
    {
        public DbEntityCountByIdCommandFactory(IInternalSession internalSession, TEntity entity)
            : base(internalSession, entity)
        {
        }

        protected override DbCommand Make(DbCommand dbCommand, TEntity entity)
        {
            entity.ValidateEntity(On.SelectById);

            dbCommand.CommandText = entity.Table.CommandsHolder.CountById;
            dbCommand.SetParameters(entity.Table.KeyFields, entity);

            return dbCommand;
        }
    }
}
