using System;
using System.Collections.Generic;
using VODB.Core.Execution.Executers.DbResults;
using VODB.DbLayer;
using VODB.EntityTranslation;
using VODB.ExpressionsToSql;
using VODB.QueryCompiler;
using VODB.Sessions;
using Fasterflect;
using VODB.Infrastructure;
using VODB.QueryCompiler.ExpressionPiecesToSql;

namespace VODB
{
    public interface IQuery<out TEntity> : IQuery
        where TEntity : class, new()
    {
        IEnumerable<TEntity> Execute(ISession session);
    }

    public interface IQuery : IQueryCondition
    {
        ITable Table { get; }
        IVodbCommand CachedCommand { get; set; }
        IQueryCondition WhereCompile { get; }

        ISqlCompiler SqlCompiler { get; }
    }

    public static class Param
    {
        public static TResult Get<TResult>()
        {
            return default(TResult);
        }
    }

    public static class Select
    {
        private static IQueryStart _All = new All();
        private static IQueryStart _Count = new Count();

        public static IQueryStart All { get { return _All; } }
        public static IQueryStart Count { get { return _Count; } }
    }

}
