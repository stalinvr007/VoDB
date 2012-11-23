using System.Collections.Generic;
using System.Data.Common;
using VODB.DbLayer.DbCommands;
using VODB.DbLayer.Loaders;

namespace VODB.DbLayer.DbExecuters
{
    internal class DbEntityQueryExecuterLazy<TEntity> : DbEntityQueryExecuterBase<TEntity>
            where TEntity : Entity, new()
    {

        private readonly IEntityLoader<TEntity> _Loader;
        private readonly IInternalSession _Session;
        
        /// <summary>
        /// Initializes a new instance of the <see>
        ///                                     <cref>DbDbEntityQueryExecuterLazy{TEntity}</cref>
        ///                                   </see>  class.
        /// </summary>
        /// <param name="commandFactory"> </param>
        /// <param name="loader">The loader.</param>
        public DbEntityQueryExecuterLazy(IInternalSession session, IDbCommandFactory commandFactory, IEntityLoader<TEntity> loader)
            : base(session, commandFactory)
        {
            _Session = session;
            _Loader = loader;
        }
        
        /// <summary>
        /// Gets the entities.
        /// </summary>
        /// <param name="reader">The reader.</param>
        protected override IEnumerable<TEntity> GetEntities(DbDataReader reader)
        {
            try
            {
                while (reader.Read())
                {
                    var newTEntity = new TEntity();

                    _Loader.Load(newTEntity, reader);

                    yield return newTEntity;
                }
            }
            finally
            {
                reader.Close();
                _Session.Close();
            }
            
        }
    }
}
