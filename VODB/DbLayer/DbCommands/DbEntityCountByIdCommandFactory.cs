using System.Data.Common;
using VODB.EntityValidators;
using VODB.Extensions;

namespace VODB.DbLayer.DbCommands
{
    internal sealed class DbEntityCountByIdCommandFactory<TEntity> : DbEntityCommandFactory<TEntity>
            where TEntity :  new()
    {
        public DbEntityCountByIdCommandFactory(IInternalSession internalSession, TEntity entity)
            : base(internalSession, entity)
        {
        }

        protected override DbCommand Make(DbCommand dbCommand, TEntity entity)
        {
            var inEntity = entity as Entity;
            inEntity.ValidateEntity(On.SelectById);

            dbCommand.CommandText = inEntity.Table.CommandsHolder.CountById;
            dbCommand.SetParameters(inEntity.Table.KeyFields, inEntity);

            return dbCommand;
        }
    }
}
