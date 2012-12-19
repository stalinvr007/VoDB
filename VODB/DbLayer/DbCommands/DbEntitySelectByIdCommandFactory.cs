using System.Data.Common;
using VODB.EntityValidators;
using VODB.Extensions;

namespace VODB.DbLayer.DbCommands
{
    internal sealed class DbEntitySelectByIdCommandFactory<TEntity> : DbEntityCommandFactory<TEntity>
        where TEntity : new()
    {
        public DbEntitySelectByIdCommandFactory(IInternalSession internalSession, TEntity entity)
            : base(internalSession, entity)
        {
        }

        protected override DbCommand Make(DbCommand dbCommand, TEntity entity)
        {
            var inEntity = entity as Entity;
            inEntity.ValidateEntity(On.SelectById);

            dbCommand.CommandText = inEntity.Table.CommandsHolder.SelectById;
            dbCommand.SetParameters(inEntity.Table.KeyFields, inEntity);

            return dbCommand;
        }
    }
}