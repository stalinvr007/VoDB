using System.Data.Common;
using VODB.EntityValidators;
using VODB.Extensions;

namespace VODB.DbLayer.DbCommands
{
    internal sealed class DbEntityCountCommandFactory<TEntity> : DbEntityCommandFactory<TEntity> where TEntity : new()
    {
        public DbEntityCountCommandFactory(IInternalSession internalSession)
            : base(internalSession, new TEntity())
        { }

        protected override DbCommand Make(DbCommand dbCommand, TEntity entity)
        {
            var inEntity = entity as Entity;
            inEntity.ValidateEntity(On.Count);

            dbCommand.CommandText = inEntity.Table.CommandsHolder.Count;
            return dbCommand;
        }
    }
}
