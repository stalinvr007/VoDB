using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VODB.DbLayer;
using VODB.EntityMapping;
using VODB.EntityTranslation;
using VODB.Infrastructure;

namespace VODB.Executors
{
    class SimpleExecutor : IExecutor
    {
        private const string SELECT_IDENTITY = "; Select @@IDENTITY";
        
        private readonly IDbCommandExecutor<int> _NonQueryExecutor;
        private readonly IDbCommandExecutor<IDataReader> _QueryExecutor;
        private readonly IDbCommandExecutor<Object> _ScalarExecutor;
        private readonly IDbCommandFactory _CommandFactory;
        private readonly IEntityMapper _Mapper;
        private readonly IEntityTranslator _Translator;
        private readonly IDbParameterFactory _ParameterFactory;
        private readonly IDbParameterFactory _OldParameterFactory;
        
        public SimpleExecutor(
            IDbCommandExecutor<int> nonQueryExecutor,
            IDbCommandExecutor<IDataReader> queryExecutor,
            IDbCommandExecutor<Object> scalarExecutor,
            IDbCommandFactory commandFactory,
            IDbParameterFactory parameterFactory,
            IDbParameterFactory oldParameterFactory,
            IEntityMapper mapper,
            IEntityTranslator translator)
        {
            _OldParameterFactory = oldParameterFactory;
            _ParameterFactory = parameterFactory;
            _Translator = translator;
            _Mapper = mapper;
            _CommandFactory = commandFactory;
            _ScalarExecutor = scalarExecutor;
            _QueryExecutor = queryExecutor;
            _NonQueryExecutor = nonQueryExecutor;
        }

        private void AddKeyFieldsToCommand(DbCommand cmd, ITable table, Object entity)
        {
            cmd.Parameters.AddRange(
                table.Keys
                    .Select(f => _ParameterFactory.CreateParameter(cmd, f, entity))
                    .ToArray()
            );
        }

        private void AddFieldsToCommand(DbCommand cmd, ITable table, Object entity)
        {
            cmd.Parameters.AddRange(
                table.Fields
                    .Select(f => _ParameterFactory.CreateParameter(cmd, f, entity))
                    .ToArray()
            );
        }

        private void AddOldFieldsToCommand(DbCommand cmd, ITable table, Object entity)
        {
            cmd.Parameters.AddRange(
                table.Keys
                    .Select(f => _OldParameterFactory.CreateParameter(cmd, f, entity))
                    .ToArray()
            );
        }

        private DbCommand GetCmd<T>(T entity, 
            Func<ITable, String> getCommandTxt, 
            Action<DbCommand, ITable> setFields, out ITable table)
        {
            table = _Translator.Translate(typeof(T));
            var cmd = _CommandFactory.MakeCommand();
            cmd.CommandText = getCommandTxt(table);

            setFields(cmd, table);
            
            return cmd;
        }

        public void Delete<T>(T entity)
        {
            ITable table;
            DbCommand cmd = GetCmd(entity, 
                t => t.SqlDeleteById,
                (c, t) => AddKeyFieldsToCommand(c, t, entity),
                out table
            );

            _NonQueryExecutor.ExecuteCommand(cmd);
        }

        public void Update<T>(T entity)
        {
            ITable table;
            DbCommand cmd = GetCmd(entity,
                t => t.SqlUpdate,
                (c, t) => { 
                    AddFieldsToCommand(c, t, entity);
                    AddOldFieldsToCommand(c, t, entity);
                },
                out table
            );

            _NonQueryExecutor.ExecuteCommand(cmd);
        }
        
        public T Insert<T>(T entity)
        {
            ITable table;
            DbCommand cmd = GetCmd(entity,
                t => t.SqlInsert,
                (c, t) => AddFieldsToCommand(c, t, entity),
                out table
            );

            if (table.IdentityField != null)
            {
                cmd.CommandText += SELECT_IDENTITY;
                table.SetIdentityValue(entity, _ScalarExecutor.ExecuteCommand(cmd));
            }
            else
            {
                _NonQueryExecutor.ExecuteCommand(cmd);
            }

            return entity;
        }
        
        public bool Exists<T>(T entity)
        {
            throw new NotImplementedException();
        }
        
        public T Query<T>(T entity)
        {
            ITable table;
            DbCommand cmd = GetCmd(entity,
                t => t.SqlSelectById,
                (c, t) => AddKeyFieldsToCommand(c, t, entity),
                out table
            );

            var reader = _QueryExecutor.ExecuteCommand(cmd);
            if (reader.Read())
            {
                _Mapper.Map(entity, table, reader);
            }

            return entity;
        }
        
        public IEnumerable<T> Query<T>()
        {
            throw new NotImplementedException();
        }
    }
}
