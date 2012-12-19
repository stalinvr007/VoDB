using System.Data.Common;
using System.Linq;
using VODB.EntityValidators;
using VODB.Extensions;

namespace VODB.DbLayer.DbCommands
{
    internal sealed class DbEntityInsertCommandFactory<TEntity> : DbEntityCommandFactory<TEntity>
        where TEntity : new()
    {
        public DbEntityInsertCommandFactory(IInternalSession internalSession, TEntity entity)
            : base(internalSession, entity)
        {
        }

        protected override DbCommand Make(DbCommand dbCommand, TEntity entity)
        {
            var inEntity = entity as Entity;
            inEntity.ValidateEntity(On.Insert);

            dbCommand.CommandText = inEntity.Table.CommandsHolder.Insert;
            dbCommand.SetParameters(
                inEntity.Table.Fields.Where(field => !field.IsIdentity),
                inEntity);

            return dbCommand;
        }
    }
}