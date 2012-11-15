using System.Data.Common;
using VODB.EntityValidators;
using VODB.Extensions;

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
            entity.ValidateEntity(On.Select);

            dbCommand.CommandText = entity.Table.CommandsHolder.Select;
            return dbCommand;
        }
    }
}