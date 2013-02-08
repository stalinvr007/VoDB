using System;
using System.Data.Common;
using VODB.Core.Execution.Statements;
using VODB.Core.Infrastructure;

namespace VODB.Core.Execution.Executers
{
    internal class CountExecuter : StatementExecuterBase<int>
    {
        public CountExecuter(IStatementGetter getter) : base(getter)
        {
        }

        protected override int Execute<TEntity>(DbCommand cmd, Table table, TEntity entity, IInternalSession session)
        {
            return Convert.ToInt32(session.RefreshCommand(cmd).ExecuteScalar());
        }
    }

    internal class CountByIdExecuter : StatementExecuterBase<int>
    {
        public CountByIdExecuter(IStatementGetter getter) : base(getter)
        {
        }

        protected override int Execute<TEntity>(DbCommand cmd, Table table, TEntity entity, IInternalSession session)
        {
            cmd.SetParameters(table.KeyFields, entity);
            return Convert.ToInt32(session.RefreshCommand(cmd).ExecuteScalar());
        }
    }

    internal class IdentityExecuter : StatementExecuterBase<Object>
    {
        public IdentityExecuter(IStatementGetter getter) : base(getter)
        {
        }

        protected override Object Execute<TEntity>(DbCommand cmd, Table table, TEntity entity, IInternalSession session)
        {
            return session.RefreshCommand(cmd).ExecuteScalar();
        }
    }
}