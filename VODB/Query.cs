using System;
using System.Collections.Generic;
using VODB.Core.Execution.Executers.DbResults;
using VODB.Sessions;

namespace VODB
{
    public interface IQuery<out TEntity>
    {
        IEnumerable<TEntity> Execute(ISession session);
    }

    public static class Query<TEntity> where TEntity : class, new()
    {
        private static ISession _Session = new SessionStub();

        public static IQuery<TEntity> PreCompile(Func<IDbQueryResult<TEntity>, IEnumerable<TEntity>> func)
        {
            return PreCompile_QueryCompiler(func);
        }

        internal static IQuery<TEntity> PreCompile_QueryCompiler(Func<IDbQueryResult<TEntity>, IEnumerable<TEntity>> func)
        {
            return new QueryCompiler.QueryCompiler<TEntity>(func(_Session.GetAll<TEntity>()));
        }

    }
}
