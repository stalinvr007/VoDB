using System.Data;
using System.Data.Common;
using VODB.Core.Execution.Statements;
using VODB.Core.Infrastructure;

namespace VODB.Core.Execution.Executers
{
    internal class SelectByIdExecuter : StatementExecuterBase<IDataReader>
    {
        public SelectByIdExecuter(IStatementGetter getter) : base(getter)
        {
        }

        protected override IDataReader Execute<TEntity>(DbCommand cmd, Table table, TEntity entity,
                                                         IInternalSession session)
        {
            cmd.SetParameters(table.KeyFields, entity);
            return session.RefreshCommand(cmd).ExecuteReader();
        }
    }
}