using System.Data.Common;
using VODB.Extensions;

namespace VODB.DbLayer.DbCommands
{
    internal sealed class DbEntitySelectByIdCommandFactory<TEntity> : DbEntityCommandFactory<TEntity>
        where TEntity : DbEntity, new()
    {
        public DbEntitySelectByIdCommandFactory(ISessionInternal session, TEntity entity)
            : base(session, entity)
        {
        }

        protected override DbCommand Make(DbCommand dbCommand, TEntity entity)
        {
            dbCommand.CommandText = entity.Table.CommandsHolder.SelectById;
            dbCommand.SetParameters(entity.Table.KeyFields, entity);

            return dbCommand;
        }
    }
}