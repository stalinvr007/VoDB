using System;
using System.Data.Common;
using VODB.Core.Infrastructure;
using VODB.Core.Execution.Statements;

namespace VODB.Core.Execution.Executers
{
    class CountExecuter : StatementExecuterBase<int>
    {
        public CountExecuter(IStatementGetter getter) : base(getter) { }

        protected override int Execute<TEntity>(DbCommand cmd, Table table, TEntity entity)
        {
            return Convert.ToInt32(cmd.ExecuteScalar());
        }
    }

    class CountByIdExecuter : StatementExecuterBase<int>
    {
        public CountByIdExecuter(IStatementGetter getter) : base(getter) { }

        protected override int Execute<TEntity>(DbCommand cmd, Table table, TEntity entity)
        {
            cmd.SetParameters(table.KeyFields, entity);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }
    }

    class IdentityExecuter : StatementExecuterBase<Object>
    {
        public IdentityExecuter(IStatementGetter getter) : base(getter) { }

        protected override Object Execute<TEntity>(DbCommand cmd, Table table, TEntity entity)
        {
            return cmd.ExecuteScalar();
        }
    }
}
