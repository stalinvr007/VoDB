using System.Collections.Generic;
using VODB.EntityTranslation;
using VODB.Expressions;
using VODB.ExpressionsToSql;
using VODB.QueryCompiler.ExpressionPiecesToSql;

namespace VODB.QueryCompiler
{
    public interface IQueryStart
    {
        IQueryCompilerLevel1<TEntity> From<TEntity>() where TEntity : class, new();
    }

    abstract class QueryStart : IQueryStart
    {
        protected static readonly IEntityTranslator _Translator = new EntityTranslator();
        protected static readonly IExpressionBreaker _Breaker = new ExpressionBreaker(_Translator);

        public abstract IQueryCompilerLevel1<TEntity> From<TEntity>() where TEntity : class, new();

        internal static IQueryCompilerLevel1<TEntity> From<TEntity>(IInternalSession session) where TEntity : class, new()
        {
            return new SelectAllFrom<TEntity>(_Translator, _Breaker, session);
        }

        internal static IQueryCompilerLevel1<TEntity> From<TEntity>(IInternalSession session, ISqlCompiler conditions, IEnumerable<IQueryParameter> parameters) where TEntity : class, new()
        {
            return new SelectAllFrom<TEntity>(_Translator, _Breaker, session, conditions, parameters);
        }
    }

    class All : QueryStart
    {
        public override IQueryCompilerLevel1<TEntity> From<TEntity>()
        {
            return new SelectAllFrom<TEntity>(_Translator, _Breaker, null);
        }
    }

}
