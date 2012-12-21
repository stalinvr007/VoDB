using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using VODB.Core.Execution.Statements;
using VODB.Core.Infrastructure;

namespace VODB.Core.Execution.Executers
{
    class SelectByIdExecuter : StatementExecuterBase<DbDataReader>
    {
        public SelectByIdExecuter(IStatementGetter getter) : base(getter) { }

        protected override DbDataReader Execute<TEntity>(DbCommand cmd, Table table, TEntity entity)
        {
            cmd.SetParameters(table.KeyFields, entity);
            return cmd.ExecuteReader();
        }
    }

}
