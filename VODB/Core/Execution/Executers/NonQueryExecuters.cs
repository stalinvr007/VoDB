using System.Data.Common;
using VODB.Core.Execution.Statements;
using VODB.Core.Infrastructure;

namespace VODB.Core.Execution.Executers
{

    internal class InsertExecuter : StatementNonQueryExecuterBase<int>
    {
        public InsertExecuter(IStatementGetter getter) : base(getter) { }

        protected override int Execute<TEntity>(DbCommand cmd, Table table, TEntity entity)
        {
            cmd.SetParameters(table.Fields, entity);
            return cmd.ExecuteNonQuery();
        }
    }

    internal class DeleteExecuter : StatementNonQueryExecuterBase<int>
    {
        public DeleteExecuter(IStatementGetter getter) : base(getter) { }

        protected override int Execute<TEntity>(DbCommand cmd, Table table, TEntity entity)
        {
            cmd.SetParameters(table.KeyFields, entity);
            return cmd.ExecuteNonQuery();
        }
    }

    internal class UpdateExecuter : StatementNonQueryExecuterBase<int>
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
