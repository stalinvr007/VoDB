using System;
using System.Data.Common;
using VODB.Core.Infrastructure;
using VODB.Core.Execution.Statements;

namespace VODB.Core.Execution.Executers
{
    class CountExecuter : StatementExecuterBase<int>
    {
        public CountExecuter(IStatementGetter getter) : base(getter) { }

        protected override int Execute<TEntity>(DbCommand cmd, Table table, TEntity entity, IInternalSession session)
        {
            return Convert.ToInt32(cmd.ExecuteScalar());
        }
    }

    class CountByIdExecuter : StatementExecuterBase<int>
    {
        public CountByIdExecuter(IStatementGetter getter) : base(getter) { }

        protected override int Execute<TEntity>(DbCommand cmd, Table table, TEntity entity, IInternalSession session)
        {
            cmd.SetParameters(table.KeyFields, entity);
            session.Open();
            return Convert.ToInt32(cmd.ExecuteScalar());
        }
    }

    class IdentityExecuter : StatementExecuterBase<Object>
    {
        public IdentityExecuter(IStatementGetter getter) : base(getter) { }

        protected override Object Execute<TEntity>(DbCommand cmd, Table table, TEntity entity, IInternalSession session)
        {
            return cmd.ExecuteScalar();
        }
    }
}
