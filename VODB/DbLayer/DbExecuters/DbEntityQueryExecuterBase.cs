using System;
using System.Collections.Generic;
using System.Data.Common;
using VODB.DbLayer.DbCommands;
using VODB.DbLayer.DbResults;
using VODB.Exceptions;

namespace VODB.DbLayer.DbExecuters
{
    /// <summary>
    /// Implements a base for a Query that returns Entities.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    internal abstract class DbEntityQueryExecuterBase<TEntity> : IQueryExecuter<TEntity>
        where TEntity : Entity, new()
    {

        private IDbCommandFactory _CommandFactory;
        private readonly IInternalSession _Session;
        
        protected DbEntityQueryExecuterBase(IInternalSession session, IDbCommandFactory commandFactory)
        {
            _Session = session;
            _CommandFactory = commandFactory;
        }

        public IDbQueryResult<TEntity> Execute()
        {
            var cmdFact = _CommandFactory;
            _CommandFactory = new DbQueryResult<TEntity>(cmdFact, this);
            return _CommandFactory as IDbQueryResult<TEntity>;
        }

        /// <summary>
        /// Gets the entities.
        /// </summary>
        /// <param name="reader">The reader.</param>
        protected abstract IEnumerable<TEntity> GetEntities(DbDataReader reader);

        public IEnumerator<TEntity> GetEnumerator()
        {
            return InternalExecute().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        public IEnumerable<TEntity> InternalExecute()
        {
            var cmd = _CommandFactory.Make();
            try
            {
                _Session.Open();
                return GetEntities(cmd.ExecuteReader());
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new UnableToExecuteQueryException(cmd.CommandText, ex);
            }
        }
    }
}
