using System.Linq;
using VODB.DbLayer;
using VODB.DbLayer.DbCommands;
using VODB.DbLayer.DbExecuters;
using VODB.DbLayer.DbResults;
using VODB.DbLayer.Loaders;

namespace VODB.Sessions
{
    public class LazySession : PublicSessionBase
    {
        public LazySession(IDbConnectionCreator creator = null)
            : base(new InternalLazySession(creator))
        { }
    }

    internal class InternalLazySession : InternalSession
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EagerSession"/> class.
        /// </summary>
        /// <param name="creator">The creator.</param>
        internal InternalLazySession(IDbConnectionCreator creator = null)
            : base(creator)
        {
        }

        public override IDbQueryResult<TEntity> GetAll<TEntity>() 
        {
            return Run(() => new DbEntityQueryExecuterLazy<TEntity>(this,
                                         new DbEntitySelectCommandFactory<TEntity>(this),
                                         new FullEntityLoader<TEntity>(this)
                                         ).Execute());
        }

        public override TEntity GetById<TEntity>(TEntity entity)
        {
            return Run(() => new DbEntityQueryExecuterLazy<TEntity>(this,
                                         new DbEntitySelectByIdCommandFactory<TEntity>(this, entity),
                                         new FullEntityLoader<TEntity>(this)
                                         ).Execute().FirstOrDefault());
        }
    }
}
