using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VODB.DbLayer;
using VODB.EntityMapping;
using VODB.EntityTranslation;
using VODB.Infrastructure;

namespace VODB.Executors
{
    class DeleteExecutor : CommandExecutor, IDeleteExecutor
    {
        private readonly IDbCommandExecutor<int> _Executor;

        public DeleteExecutor(
            IDbCommandExecutor<int> executor, 
            IEntityTranslator translator,
            IDbCommandFactory factory,
            IDbParameterFactory parameterFactory,
            IDbParameterFactory oldParameterFactory)
            : base(translator, factory, parameterFactory, oldParameterFactory)
        {
            _Executor = executor;
        }

        public void Delete<TEntity>(TEntity entity)
        {
            ITable table = GetTable<TEntity>();

            _Executor.ExecuteCommand(
                AddKeyFieldsToCommand(
                    CreateCommand(table.SqlDeleteById),
                    table,
                    entity
                )
            );
        }
    }

    /// <summary>
    /// Executor for the Delete Command.
    /// </summary>
    public interface IDeleteExecutor
    {
        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        void Delete<TEntity>(TEntity entity);
    }
}
