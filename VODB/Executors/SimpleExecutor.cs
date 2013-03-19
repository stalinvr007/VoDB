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
        
        private readonly IDbCommandExecutor<int> _NonQueryExecutor;
        private readonly IDbCommandExecutor<IDataReader> _QueryExecutor;
        private readonly IDbCommandExecutor<Object> _ScalarExecutor;
        private readonly IDbCommandFactory _CommandFactory;
        private readonly IEntityMapper _Mapper;
        private readonly IEntityTranslator _Translator;
        private readonly IDbParameterFactory _ParameterFactory;
        
        public SimpleExecutor(
            IDbCommandExecutor<int> nonQueryExecutor,
            IDbCommandExecutor<IDataReader> queryExecutor,
            IDbCommandExecutor<Object> scalarExecutor,
            IDbCommandFactory commandFactory,
            IDbParameterFactory parameterFactory,
            IEntityMapper mapper,
            IEntityTranslator translator)
        {
            _ParameterFactory = parameterFactory;
            _Translator = translator;
            _Mapper = mapper;
            _CommandFactory = commandFactory;
            _ScalarExecutor = scalarExecutor;
            _QueryExecutor = queryExecutor;
            _NonQueryExecutor = nonQueryExecutor;
        }

        private void AddFieldsToCommand(DbCommand cmd, ITable table, Object entity)
        {
            cmd.Parameters.AddRange(
                table.Fields
                    .Select(f => _ParameterFactory.CreateParameter(cmd, f, entity))
                    .ToArray()
            );
        }

        public void Delete<T>(T entity)
        {
            var table = _Translator.Translate(typeof(T));
            var cmd = _CommandFactory.MakeCommand();
            cmd.CommandText = table.SqlDeleteById;

            AddFieldsToCommand(cmd, table, entity);

            _NonQueryExecutor.ExecuteCommand(cmd);
        }

        public void Update<T>(T entity)
        {
            throw new NotImplementedException();
        }
        
        public T Insert<T>(T entity)
        {
            throw new NotImplementedException();
        }
        
        public bool Exists<T>(T entity)
        {
            throw new NotImplementedException();
        }
        
        public T Query<T>(T entity)
        {
            throw new NotImplementedException();
        }
        
        public IEnumerable<T> Query<T>()
        {
            throw new NotImplementedException();
        }
    }
}
