using System.Data.Common;
using VODB.Core.Execution.Statements;
using VODB.Core.Infrastructure;

namespace VODB.Core.Execution.Executers
{
    internal class SelectByIdExecuter : StatementExecuterBase<DbDataReader>
    {
        public SelectByIdExecuter(IStatementGetter getter) : base(getter)
        {
        }

        protected override DbDataReader Execute<TEntity>(DbCommand cmd, Table table, TEntity entity,
                                                         IInternalSession session)
        {
            cmd.SetParameters(table.KeyFields, entity);
            return session.RefreshCommand(cmd).ExecuteReader();
        }
    }
}