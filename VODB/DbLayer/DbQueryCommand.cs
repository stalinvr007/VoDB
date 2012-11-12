using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using VODB.VirtualDataBase;

namespace VODB.DbLayer
{

    internal sealed class DbQueryResult
    {

        public DbQueryResult(IEnumerable<Object> values)
        {
            Values = values;
        }

        public IEnumerable<Object> Values { get; private set; }

    }

    internal sealed class DbQueryCommand : IQuery<DbQueryResult>
    {

        private readonly DbCommand _Query;
        private readonly Table _Table;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbQueryCommand" /> class.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="table"> </param>
        public DbQueryCommand(DbCommand query, Table table)
        {
            _Table = table;
            _Query = query;
        }


        public IEnumerable<DbQueryResult> Execute()
        {
            var reader = _Query.ExecuteReader();

            while (reader.Read())
            {
                yield return new DbQueryResult(GetValues(_Table.Fields, reader));
            }
            reader.Close();
        }

        private IEnumerable<object> GetValues(IEnumerable<Field> fields, DbDataReader reader)
        {
            return fields.Select(f => reader[f.FieldName]).ToList();
        }

    }
}
