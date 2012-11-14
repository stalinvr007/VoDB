using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using VODB.VirtualDataBase;

namespace VODB.DbLayer.DbExecuters
{
    internal sealed class DbQueryExecuterCommandEager : IQueryExecuter<DbQueryResult>
    {
        private readonly DbCommand _Query;
        private readonly Table _Table;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbQueryExecuterCommandEager" /> class.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="table"> </param>
        public DbQueryExecuterCommandEager(DbCommand query, Table table)
        {
            _Table = table;
            _Query = query;
        }

        #region IQueryExecuter<DbQueryResult> Members

        public IEnumerable<DbQueryResult> Execute()
        {
            DbDataReader reader = _Query.ExecuteReader();

            while (reader.Read())
            {
                yield return new DbQueryResult(GetValues(_Table.Fields, reader));
            }
            reader.Close();
        }

        #endregion

        private IEnumerable<object> GetValues(IEnumerable<Field> fields, DbDataReader reader)
        {
            return fields.Select(f => reader[f.FieldName]).ToList();
        }
    }
}