using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
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

        public TResult Execute<TEntity>(TEntity entity, IInternalSession session)
        {
            var cmd = session.CreateCommand();

            session.Open();
            var table = Engine.GetTable(entity.GetType());
            cmd.CommandText = _Getter.GetStatement(table.CommandsHolder);
            
            return Execute(cmd, table, entity);
        }

        protected abstract TResult Execute<TEntity>(DbCommand cmd, Table table, TEntity entity);

    }
}