using System;
using System.Collections.Generic;
using System.Linq;
using VODB.DbLayer;
using VODB.DbLayer.DbCommands;
using VODB.DbLayer.DbExecuters;
using VODB.DbLayer.Loaders;

namespace VODB.Sessions
{

    public class EagerSession : PublicSessionBase
    {
        public EagerSession(IDbConnectionCreator creator = null)
            : base(new InternalEagerSession(creator))
        { }   
    }

    internal class InternalEagerSession : InternalSession
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EagerSession"/> class.
        /// </summary>
        /// <param name="creator">The creator.</param>
        internal InternalEagerSession(IDbConnectionCreator creator = null)
            : base(creator)
        {
        }

        public override IDbQueryResult<TEntity> GetAll<TEntity>()
        {
            return Run(() => new DbEntityQueryExecuterEager<TEntity>(
                                         new DbEntitySelectCommandFactory<TEntity>(this),
                                         new FullEntityLoader<TEntity>(this)
                                         ).Execute());
        }

        public override TEntity GetById<TEntity>(TEntity entity)
        {
            return Run(() => new DbEntityQueryExecuterEager<TEntity>(
                                         new DbEntitySelectByIdCommandFactory<TEntity>(this, entity),
                                         new FullEntityLoader<TEntity>(this)
                                         ).Execute().FirstOrDefault());
        }
    }
}