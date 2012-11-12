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
    /// Implements a base for a Query that returns Entities.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    internal abstract class EntityQueryBase<TEntity> : IQuery<TEntity>
        where TEntity : DbEntity, new()
    {
        private readonly DbConnection _Connection;

        public EntityQueryBase(DbConnection connection)
        {
            _Connection = connection;
        }

        public IEnumerable<TEntity> Execute()
        {
            var cmd = _Connection.CreateCommand();
            cmd.CommandText = new TEntity().Table.CommandsHolder.Select;
            try
            {
                return GetEntities(cmd.ExecuteReader());
            }
            catch (Exception ex)
            {
                throw new UnableToExecuteQueryException(cmd.CommandText, ex);
            }
        }

        /// <summary>
        /// Gets the entities.
        /// </summary>
        /// <param name="reader">The reader.</param>
        protected abstract IEnumerable<TEntity> GetEntities(DbDataReader reader);

    }
}
