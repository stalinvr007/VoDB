﻿using System.Data.Common;
using VODB.EntityValidators;
using VODB.Extensions;

namespace VODB.DbLayer.DbCommands
{
    internal sealed class DbEntityUpdateCommandFactory<TEntity> : DbEntityCommandFactory<TEntity> 
        where TEntity : new()
    {
        public DbEntityUpdateCommandFactory(IInternalSession internalSession, TEntity entity)
            : base(internalSession, entity)
        {
        }

        protected override DbCommand Make(DbCommand dbCommand, TEntity entity)
        {
            var inEntity = entity as Entity;
            inEntity.ValidateEntity(On.Update);

            dbCommand.CommandText = inEntity.Table.CommandsHolder.Update;
            dbCommand.SetParameters(inEntity.Table.Fields, inEntity);
            dbCommand.SetOldParameters(inEntity);

            return dbCommand;
        }
    }
}
