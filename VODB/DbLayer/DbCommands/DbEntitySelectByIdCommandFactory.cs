using System.Data.Common;
using VODB.EntityValidators;
using VODB.Extensions;

namespace VODB.DbLayer.DbCommands
{
    internal sealed class DbEntitySelectByIdCommandFactory<TEntity> : DbEntityCommandFactory<TEntity>
        where TEntity : DbEntity, new()
    {
        public DbEntitySelectByIdCommandFactory(IInternalSession internalSession, TEntity entity)
            : base(internalSession, entity)
        {
        }

        protected override DbCommand Make(DbCommand dbCommand, TEntity entity)
        {
            entity.ValidateEntity(On.SelectById);

            dbCommand.CommandText = entity.Table.CommandsHolder.SelectById;
            dbCommand.SetParameters(entity.Table.KeyFields, entity);

            return dbCommand;
        }
    }
}