using System;
using System.Data.Common;
using VODB.Core.Execution.Statements;
using VODB.Core.Infrastructure;

namespace VODB.Core.Execution.Executers
{
    internal abstract class StatementExecuterBase<TResult> : IStatementExecuter<TResult>
    {
        private readonly IStatementGetter _Getter;

        public StatementExecuterBase(IStatementGetter getter)
        {
            _Getter = getter;
        }

        #region IStatementExecuter<TResult> Members

        public TResult Execute<TEntity>(TEntity entity, IInternalSession session)
        {
            try
            {
                session.Open();
                DbCommand cmd = session.CreateCommand();
                Table table = Engine.GetTable(entity.GetType());
                cmd.CommandText = _Getter.GetStatement(table.CommandsHolder);

                return Execute(cmd, table, entity, session);
            }
            catch (Exception ex)
            {
                ex.Handle();
            }
            return default(TResult);
        }

        #endregion

        protected abstract TResult Execute<TEntity>(DbCommand cmd, Table table, TEntity entity, IInternalSession session);
    }
}