using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using VODB.DbLayer.Loaders;
using VODB.Exceptions;

namespace VODB.DbLayer
{

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    internal class EntityQueryEager<TEntity> : EntityQueryBase<TEntity>
        where TEntity : DbEntity, new()
    {
        private readonly EntityLoader<TEntity> _Loader;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityQuery{TEntity}" /> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        public EntityQueryEager(DbConnection connection, EntityLoader<TEntity> loader)
            : base(connection)
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
