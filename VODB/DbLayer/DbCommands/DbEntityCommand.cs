using System.Collections.Generic;
using System.Data.Common;
using VODB.VirtualDataBase;

namespace VODB.DbLayer.DbCommands
{
    internal abstract class DbEntityCommand<TEntity> : DbCommandFactory
        where TEntity : DbEntity, new()
    {
        private readonly TEntity _entity;

        protected DbEntityCommand(DbConnection connection, TEntity entity)
            : base(connection)
        {
            _entity = entity;
        }

        protected void SetParameters(DbCommand dbCommand, IEnumerable<Field> fields)
        {
            
        }

        protected override DbCommand Make(DbCommand dbCommand)
        {
            return Make(dbCommand, _entity);
        }

        protected abstract DbCommand Make(DbCommand dbCommand, TEntity entity);
    }
}
