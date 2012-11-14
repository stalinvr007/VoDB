using System.Data.Common;

namespace VODB.DbLayer.DbCommands
{
    internal abstract class DbEntityCommandFactory<TEntity> : DbCommandFactory
        where TEntity : DbEntity, new()
    {
        private readonly TEntity _entity;

        protected DbEntityCommandFactory(ISessionInternal session, TEntity entity)
            : base(session)
        {
            _entity = entity;
        }

        protected override DbCommand Make(DbCommand dbCommand)
        {
            return Make(dbCommand, _entity);
        }

        protected abstract DbCommand Make(DbCommand dbCommand, TEntity entity);
    }
}