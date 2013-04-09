using System;
using System.Collections.Generic;
using VODB.DbLayer;
using VODB.ExpressionsToSql;
using VODB.QueryCompiler;
using VODB.Infrastructure;
using VODB.QueryCompiler.ExpressionPiecesToSql;

namespace VODB
{
    public interface IQuery<out TEntity>
        where TEntity : class, new()
    {
        IEnumerable<TEntity> Execute(ISession session);
    }

    internal interface IQuery : IQueryCondition
    {
        ITable Table { get; }
        IVodbCommand CachedCommand { get; set; }
        IQueryCondition WhereCompile { get; }
        ISqlCompiler SqlCompiler { get; }

        Func<IField, object, string> AddParameter { get; set; }
        Object InternalWhere(IField field, IQueryParameter parameter);
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

        public static IQueryStart All { get { return _All; } }
    }

}
