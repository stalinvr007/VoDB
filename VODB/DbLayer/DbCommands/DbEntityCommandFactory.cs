using System.Data.Common;
using VODB.Core.Execution.Factories;

namespace VODB.DbLayer.DbCommands
{
    internal abstract class DbEntityCommandFactory<TEntity> : DbCommandFactory
        where TEntity : new()
    {
        private readonly TEntity _entity;

        protected DbEntityCommandFactory(IInternalSession internalSession, TEntity entity)
            : base(internalSession)
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