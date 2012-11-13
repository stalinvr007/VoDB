using System.Collections.Generic;
using System.Data.Common;
using VODB.DbLayer.Loaders;
using VODB.DbLayer.DbCommands;

namespace VODB.DbLayer.DbExecuters
{

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    internal class DbEntityQueryExecuterEager<TEntity> : DbEntityQueryExecuterBase<TEntity>
        where TEntity : DbEntity, new()
    {
        private readonly EntityLoader<TEntity> _Loader;


        /// <summary>
        /// Initializes a new instance of the <see cref="DbEntityQueryExecuterEager{TEntity}" /> class.
        /// </summary>
        /// <param name="commandFactory">The command factory.</param>
        /// <param name="loader">The loader.</param>
        public DbEntityQueryExecuterEager(IDbCommandFactory commandFactory, EntityLoader<TEntity> loader)
            : base(commandFactory)
        {
            _Loader = loader;
        }


        /// <summary>
        /// Gets the entities.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        protected override IEnumerable<TEntity> GetEntities(DbDataReader reader)
        {
            ICollection<TEntity> entities = new LinkedList<TEntity>();

            try
            {
                while (reader.Read())
                {
                    TEntity newTEntity = new TEntity();
                    
                    _Loader.Load(newTEntity, reader);

                    entities.Add(newTEntity);
                }
            }
            finally
            {
                reader.Close();
            }

            return entities;
        }
    }
}
