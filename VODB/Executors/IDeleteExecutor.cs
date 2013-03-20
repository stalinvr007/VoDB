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
    abstract class CommandExecutor
    {

        private readonly IEntityTranslator _Translator;
        private readonly IDbCommandFactory _Factory;
        private readonly IDbParameterFactory _ParameterFactory;
        private readonly IDbParameterFactory _OldParameterFactory;

        protected CommandExecutor(
            IEntityTranslator translator, 
            IDbCommandFactory factory,
            IDbParameterFactory parameterFactory,
            IDbParameterFactory oldParameterFactory)
        {
            _Factory = factory;
            _Translator = translator;
            _ParameterFactory = parameterFactory;
            _OldParameterFactory = oldParameterFactory;
        }

        protected ITable GetTable<TEntity>()
        {
            Debug.Assert(typeof(TEntity) != typeof(Object));

            return _Translator.Translate(typeof(TEntity));
        }

        protected DbCommand CreateCommand(String cmd)
        {
            DbCommand command = _Factory.MakeCommand();
            command.CommandText = cmd;
            return command;
        }

        protected DbCommand AddKeyFieldsToCommand(DbCommand cmd, ITable table, Object entity)
        {
            cmd.Parameters.AddRange(
                table.Keys
                    .Select(f => _ParameterFactory.CreateParameter(cmd, f, entity))
                    .ToArray()
            );
            return cmd;
        }

        protected DbCommand AddFieldsToCommand(DbCommand cmd, ITable table, Object entity)
        {
            cmd.Parameters.AddRange(
                table.Fields
                    .Select(f => _ParameterFactory.CreateParameter(cmd, f, entity))
                    .ToArray()
            );
            return cmd;
        }

        protected DbCommand AddOldFieldsToCommand(DbCommand cmd, ITable table, Object entity)
        {
            cmd.Parameters.AddRange(
                table.Keys
                    .Select(f => _OldParameterFactory.CreateParameter(cmd, f, entity))
                    .ToArray()
            );
            return cmd;
        }

    }

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

        public void Delete<TEntity>(TEntity entity) where TEntity : new()
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
        void Delete<TEntity>(TEntity entity) where TEntity : new();
    }
}
