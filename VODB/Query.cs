using System;
using System.Collections.Generic;
using VODB.Sessions;

namespace VODB
{
    public interface IQuery<out TEntity>
    {
        IEnumerable<TEntity> Execute(ISession session);
    }

    public static class Query<TEntity> where TEntity : class, new()
    {

        public static IQuery<TEntity> PreCompile(Func<ISession, IEnumerable<TEntity>> func)
        {
            return new QueryCompiler.QueryCompiler<TEntity>(func(new SessionStub()));
        }

    }
}
