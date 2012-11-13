using System;
using System.Collections.Generic;
using System.Data.Common;
using VODB.DbLayer.DbCommands;
using VODB.Exceptions;

namespace VODB.DbLayer.DbExecuters
{
    /// <summary>
    /// Implements a base for a Query that returns Entities.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    internal abstract class DbEntityQueryExecuterBase<TEntity> : IQueryExecuter<TEntity>
        where TEntity : DbEntity, new()
    {

        private readonly IDbCommandFactory _CommandFactory;

        protected DbEntityQueryExecuterBase(IDbCommandFactory commandFactory)
        {
            _CommandFactory = commandFactory;
        }

        public IEnumerable<TEntity> Execute()
        {
            var cmd = _CommandFactory.Make();
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
