using System.Data.Common;
using VODB.EntityValidators;
using VODB.Extensions;

namespace VODB.DbLayer.DbCommands
{
    internal sealed class DbEntityCountCommandFactory<TEntity> : DbEntityCommandFactory<TEntity> where TEntity : DbEntity, new()
    {
        public DbEntityCountCommandFactory(IInternalSession internalSession)
            : base(internalSession, new TEntity())
        { }

        protected override DbCommand Make(DbCommand dbCommand, TEntity entity)
        {

            entity.ValidateEntity(On.Count);

            dbCommand.CommandText = entity.Table.CommandsHolder.Count;
            return dbCommand;
        }
    }
}
