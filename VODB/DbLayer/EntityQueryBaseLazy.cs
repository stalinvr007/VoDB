using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using VODB.DbLayer.Loaders;
using VODB.Exceptions;

namespace VODB.DbLayer
{
    internal class EntityQueryLazy<TEntity> : EntityQueryBase<TEntity>
            where TEntity : DbEntity, new()
    {

        private readonly EntityLoader<TEntity> _Loader;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityQueryLazy{TEntity}" /> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        public EntityQueryLazy(DbConnection connection, EntityLoader<TEntity> loader)
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
                    TEntity newTEntity = new TEntity();

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
