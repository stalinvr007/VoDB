using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using VODB.Core.Execution.SqlPartialBuilders;
using VODB.Core.Execution.Statements;
using VODB.Core.Infrastructure;
using VODB.Core.Loaders;
using VODB.Core.Loaders.Factories;
using VODB.ExpressionParser;

namespace VODB.Core.Execution.Executers
{
    internal class QueryExecuter : IQueryExecuter
    {

        private readonly IEntityFactory _Factory;
        private readonly IEntityLoader _Loader;
        public QueryExecuter(IEntityLoader loader, IEntityFactory factory)
        {
            _Loader = loader;
            _Factory = factory;
        }

        public IEnumerable RunQuery(Type entityType, IInternalSession session, String query, IEnumerable<Parameter> parameters)
        {

            var cmd = session.CreateCommand();

            cmd.CommandText = query;

            cmd.SetParameters(parameters.Select(p => new KeyValuePair<Key, Object>(new Key(p.Field, p.ParamName), p.Value)));

            var reader = cmd.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    Object newEntity = _Factory.Make(entityType, session);
                    _Loader.Load(newEntity, session, reader);
                    yield return newEntity;
                }
            }
            finally
            {
                reader.Close();
            }
        }
    }
}
