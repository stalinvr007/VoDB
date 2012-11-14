using System.Collections.Generic;
using VODB.DbLayer;
using VODB.DbLayer.DbCommands;
using VODB.DbLayer.DbExecuters;
using VODB.DbLayer.Loaders;

namespace VODB
{
    public sealed class StayAliveEagerSession : Session
    {

        internal StayAliveEagerSession(IDbConnectionCreator creator = null)
            :base(creator)
        {
            
        }

        public override IEnumerable<TEntity> GetAll<TEntity>()
        {
            Open();

            return new DbEntityQueryExecuterEager<TEntity>(
                new DbEntitySelectCommandFactory<TEntity>(this),
                new FullEntityLoader<TEntity>()
                ).Execute();

        }
    }
}
