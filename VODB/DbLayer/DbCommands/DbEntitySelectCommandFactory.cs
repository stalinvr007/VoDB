using System.Data.Common;
using VODB.EntityValidators;
using VODB.Extensions;

namespace VODB.DbLayer.DbCommands
{
    internal sealed class DbEntitySelectCommandFactory<TEntity> : DbEntityCommandFactory<TEntity> 
        where TEntity : new()
    {
        public DbEntitySelectCommandFactory(IInternalSession internalSession)
            : base(internalSession, new TEntity())
        {
        }

        protected override DbCommand Make(DbCommand dbCommand, TEntity entity)
        {
            var inEntity = entity as Entity;
            inEntity.ValidateEntity(On.Select);

            dbCommand.CommandText = inEntity.Table.CommandsHolder.Select;
            return dbCommand;
        }
    }
}