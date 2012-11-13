using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using VODB.Extensions;

namespace VODB.DbLayer.DbCommands
{
    internal sealed class DbEntityInsertCommandFactory<TEntity> : DbEntityCommandFactory<TEntity>
        where TEntity : DbEntity, new()
    {
        public DbEntityInsertCommandFactory(DbConnection connection, TEntity entity)
            : base(connection, entity)
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
