using System.Data.Common;
using VODB.Core.Execution.Statements;
using VODB.Core.Infrastructure;

namespace VODB.Core.Execution.Executers
{
    internal class InsertExecuter : StatementExecuterBase<int>
    {
        public InsertExecuter(IStatementGetter getter) : base(getter) { }

        protected override int Execute<TEntity>(DbCommand cmd, Table table, TEntity entity)
        {
            cmd.SetParameters(table.Fields, entity);
            return cmd.ExecuteNonQuery();
        }
    }

    internal class DeleteExecuter : StatementExecuterBase<int>
    {
        public DeleteExecuter(IStatementGetter getter) : base(getter) { }

        protected override int Execute<TEntity>(DbCommand cmd, Table table, TEntity entity)
        {
            cmd.SetParameters(table.KeyFields, entity);
            return cmd.ExecuteNonQuery();
        }
    }

    internal class UpdateExecuter : StatementExecuterBase<int>
    {
        public UpdateExecuter(IStatementGetter getter) : base(getter) { }

        protected override int Execute<TEntity>(DbCommand cmd, Table table, TEntity entity)
        {
            cmd.SetParameters(table.Fields, entity);
            cmd.SetOldParameters(table, entity);
            return cmd.ExecuteNonQuery();
        }
    }

}
