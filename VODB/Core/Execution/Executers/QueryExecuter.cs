using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using VODB.ConcurrentReader;
using VODB.Core.Execution.SqlPartialBuilders;
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

        #region IQueryExecuter Members

        public IEnumerable RunQuery(Type entityType, IInternalSession session, String query,
                                    IEnumerable<Parameter> parameters)
        {
            session.Open();
            DbCommand cmd = session.CreateCommand();

            cmd.CommandText = query;

            cmd.SetParameters(
                parameters.Select(p => new KeyValuePair<Key, Object>(new Key(p.Field, p.ParamName), p.Value)));

            IDataReader reader = cmd.ExecuteReader();

            try
            {
                return reader.AsParallel().Transform(t =>
                {
                    object newEntity = _Factory.Make(entityType, session);
                    _Loader.Load(newEntity, session, t.Reader);
                    return newEntity;
                });
            }
            finally
            {
                reader.Close();
                session.Close();
            }
        }

        #endregion
    }
}