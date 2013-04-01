using System;
using System.Collections.Generic;
using VODB.Core.Execution.Executers.DbResults;
using VODB.EntityTranslation;
using VODB.ExpressionsToSql;
using VODB.QueryCompiler;
using VODB.Sessions;

namespace VODB
{
    public interface IQuery<out TEntity> : IQueryCondition
        where TEntity : class, new()
    {
        IEnumerable<TEntity> Execute(ISession session);
    }

    public static class Param
    {
        public static TResult Get<TResult>()
        {
            return default(TResult);
        }
    }

    public static class Query<TEntity> where TEntity : class, new()
    {
        private static ISession _Session = new SessionStub();
        private static IEntityTranslator _Translator = new EntityTranslator();

        public static IQuery<TEntity> PreCompile(Func<IQueryCompilerLevel1<TEntity>, IEnumerable<TEntity>> func)
        {
            return PreCompile_QueryCompiler(func);
        }

        internal static IQuery<TEntity> PreCompile_QueryCompiler(Func<IQueryCompilerLevel1<TEntity>, IEnumerable<TEntity>> func)
        {
            return new QueryCompiler<TEntity>(_Translator, func);
        }

    }
}
