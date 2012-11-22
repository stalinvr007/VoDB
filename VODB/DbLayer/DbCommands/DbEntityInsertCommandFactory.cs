using System.Data.Common;
using System.Linq;
using VODB.EntityValidators;
using VODB.Extensions;

namespace VODB.DbLayer.DbCommands
{
    internal sealed class DbEntityInsertCommandFactory<TEntity> : DbEntityCommandFactory<TEntity>
        where TEntity : Entity, new()
    {
        public DbEntityInsertCommandFactory(IInternalSession internalSession, TEntity entity)
            : base(internalSession, entity)
        {
        }

        protected override DbCommand Make(DbCommand dbCommand, TEntity entity)
        {
            entity.ValidateEntity(On.Insert);

            dbCommand.CommandText = entity.Table.CommandsHolder.Insert;
            dbCommand.SetParameters(
                entity.Table.Fields.Where(field => !field.IsIdentity),
                entity);

            return dbCommand;
        }
    }
}