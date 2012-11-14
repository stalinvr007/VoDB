using System.Data.Common;
using System.Linq;
using VODB.Extensions;

namespace VODB.DbLayer.DbCommands
{
    internal sealed class DbEntityInsertCommandFactory<TEntity> : DbEntityCommandFactory<TEntity>
        where TEntity : DbEntity, new()
    {
        public DbEntityInsertCommandFactory(ISessionInternal session, TEntity entity)
            : base(session, entity)
        {
        }

        protected override DbCommand Make(DbCommand dbCommand, TEntity entity)
        {
            dbCommand.CommandText = entity.Table.CommandsHolder.Insert;
            dbCommand.SetParameters(
                entity.Table.Fields.Where(field => !field.IsIdentity),
                entity);

            return dbCommand;
        }
    }
}