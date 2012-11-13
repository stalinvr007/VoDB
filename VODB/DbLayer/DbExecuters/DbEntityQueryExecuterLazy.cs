using System.Collections.Generic;
using System.Data.Common;
using VODB.DbLayer.Loaders;

namespace VODB.DbLayer.DbExecuters
{
    internal class DbEntityQueryExecuterLazy<TEntity> : DbEntityQueryExecuterBase<TEntity>
            where TEntity : DbEntity, new()
    {

        private readonly EntityLoader<TEntity> _Loader;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbDbEntityQueryExecuterLazy{TEntity}" /> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="loader">The loader.</param>
        public DbEntityQueryExecuterLazy(DbConnection connection, EntityLoader<TEntity> loader)
            : base(connection)
        {
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
            }
            
        }
    }
}
