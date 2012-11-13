using System.Collections.Generic;
using System.Data.Common;
using VODB.VirtualDataBase;
using VODB.Extensions;

namespace VODB.DbLayer.DbCommands
{
    internal sealed class DbEntitySelectByIdCommandFactory<TEntity> : DbEntityCommandFactory<TEntity> 
        where TEntity : DbEntity, new()
    {
        public DbEntitySelectByIdCommandFactory(DbConnection connection, TEntity entity)
            : base(connection, entity)
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