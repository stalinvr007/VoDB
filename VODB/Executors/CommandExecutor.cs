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

        private static DbCommand AddFieldsToCommand(
            DbCommand cmd,
            IEnumerable<IField> fields,
            Object entity,
            IDbParameterFactory parameterFactory)
        {
            cmd.Parameters.AddRange(
                fields.Select(f => parameterFactory.CreateParameter(cmd, f, entity)).ToArray()
            );
            return cmd;
        }

        protected DbCommand AddKeyFieldsToCommand(DbCommand cmd, ITable table, Object entity)
        {
            return AddFieldsToCommand(cmd, table.Keys, entity, _ParameterFactory);
        }

        protected DbCommand AddFieldsToCommand(DbCommand cmd, ITable table, Object entity)
        {
            return AddFieldsToCommand(cmd, table.Fields, entity, _ParameterFactory);
        }

        protected DbCommand AddOldFieldsToCommand(DbCommand cmd, ITable table, Object entity)
        {
            return AddFieldsToCommand(cmd, table.Keys, entity, _OldParameterFactory);
        }

    }
}
