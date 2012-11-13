using System.Collections.Generic;
using System.Data.Common;
using VODB.VirtualDataBase;
using VODB.Extensions;

namespace VODB.DbLayer.DbCommands
{
    internal abstract class DbEntityCommandFactory<TEntity> : DbCommandFactory
        where TEntity : DbEntity, new()
    {
        private readonly TEntity _entity;

        protected DbEntityCommandFactory(DbConnection connection, TEntity entity)
            : base(connection)
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
