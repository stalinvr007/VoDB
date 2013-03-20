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
    class UpdateExecutor : CommandExecutor, IUpdateExecutor
    {
        private readonly IDbCommandExecutor<int> _Executor;
        public UpdateExecutor(IDbCommandExecutor<int> executor, IEntityTranslator translator, IDbCommandFactory factory, IDbParameterFactory parameterFactory, IDbParameterFactory oldParameterFactory)
            : base(translator, factory, parameterFactory, oldParameterFactory)
        {
            _Executor = executor;
            
        }        
        
        public TEntity Update<TEntity>(TEntity entity)
        {
            ITable table = GetTable<TEntity>();

            _Executor.ExecuteCommand(           /* Executes the Command */
                AddOldFieldsToCommand(          /* Appends the old values parameters */
                    AddKeyFieldsToCommand(      /* Appends the field values parameters */
                        CreateCommand(table.SqlDeleteById),
                        table,
                        entity
                    ),
                    table,
                    entity
                )
            );

            return entity;
        }
    }

    /// <summary>
    /// Executor for the Update Command
    /// </summary>
    public interface IUpdateExecutor
    {
        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        TEntity Update<TEntity>(TEntity entity);
    }
}
