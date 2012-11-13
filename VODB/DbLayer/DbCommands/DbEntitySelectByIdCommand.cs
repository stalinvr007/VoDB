using System.Collections.Generic;
using System.Data.Common;
using VODB.VirtualDataBase;

namespace VODB.DbLayer.DbCommands
{
    internal sealed class DbEntitySelectByIdCommand<TEntity> : DbEntityCommand<TEntity> where TEntity : DbEntity, new()
    {
        public DbEntitySelectByIdCommand(DbConnection connection, TEntity entity)
            : base(connection, entity)
        {
        }

        protected override DbCommand Make(DbCommand dbCommand, TEntity entity)
        {
            dbCommand.CommandText = entity.Table.CommandsHolder.SelectById;
            SetParameters(dbCommand, entity.Table.KeyFields);

            return dbCommand;
        }

        
    }
}