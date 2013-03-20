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


    class LoadByIdExecutor : CommandExecutor, ILoadByIdExecutor
    {
        private readonly IEntityMapper _Mapper;
        private readonly IDbCommandExecutor<IDataReader> _Executor;

        public LoadByIdExecutor(IDbCommandExecutor<IDataReader> executor, IEntityMapper mapper, IEntityTranslator translator, IDbCommandFactory factory, IDbParameterFactory parameterFactory, IDbParameterFactory oldParameterFactory)
            : base(translator, factory, parameterFactory, oldParameterFactory)
        {
            _Executor = executor;
            _Mapper = mapper;
            
        }

        public TEntity Load<TEntity>(TEntity entity)
        {
            ITable table = GetTable<TEntity>();

            var reader = _Executor.ExecuteCommand(
                AddKeyFieldsToCommand(
                    CreateCommand(table.SqlSelectById),
                    table,
                    entity
                )
            );

            if (reader.Read())
            {
                return _Mapper.Map(entity, table, reader);
            }

            throw new NoEntityFoundException();
        }
    }

    /// <summary>
    /// Executor for the SelectById Command
    /// </summary>
    public interface ILoadByIdExecutor
    {
        /// <summary>
        /// Loads the specified entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        TEntity Load<TEntity>(TEntity entity);
    }
}
