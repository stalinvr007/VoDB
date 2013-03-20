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
using VODB.Exceptions;
using VODB.Infrastructure;

namespace VODB.Executors
{
    class InsertExecutor : CommandExecutor, IInsertExecutor
    {
        private readonly IDbCommandExecutor<Object> _Executor;

        public InsertExecutor(IDbCommandExecutor<Object> executor, IEntityTranslator translator, IDbCommandFactory factory, IDbParameterFactory parameterFactory, IDbParameterFactory oldParameterFactory)
            : base(translator, factory, parameterFactory, oldParameterFactory)
        {
            _Executor = executor;
            
        }

        private object Execute<TEntity>(TEntity entity, ITable table, String sql)
        {
            return _Executor.ExecuteCommand(
                AddFieldsToCommand(
                    CreateCommand(table.SqlInsert),
                    table,
                    entity
                )
            );
        }

        public TEntity Insert<TEntity>(TEntity entity)
        {
            var table = GetTable<TEntity>();

            if (table.IdentityField != null)
            {
                var idValue = Execute<TEntity>(entity, table, table.SqlInsert + "; Select @@IDENTITY");

                table.IdentityField
                    .SetFieldFinalValue(entity, idValue);
            }
            else
            {
                Execute<TEntity>(entity, table, table.SqlInsert);
            }

            return entity;
        }
    }

    /// <summary>
    /// Executor for the Insert Command
    /// </summary>
    public interface IInsertExecutor
    {
        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        TEntity Insert<TEntity>(TEntity entity);
    }
}
