using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VODB.EntityTranslation;
using VODB.Expressions;

namespace VODB.QueryCompiler
{
    public interface IQueryStart
    {
        IQueryCompilerLevel1<TEntity> From<TEntity>() where TEntity : class, new();
    }

    abstract class QueryStart : IQueryStart
    {
        protected static IEntityTranslator _Translator = new EntityTranslator();
        protected static IExpressionBreaker _Breaker = new ExpressionBreaker(_Translator);

        public abstract IQueryCompilerLevel1<TEntity> From<TEntity>() where TEntity : class, new();
    }

    class All : QueryStart
    {
        public override IQueryCompilerLevel1<TEntity> From<TEntity>()
        {
            return new SelectAllFrom<TEntity>(_Translator, _Breaker);
        }
    }

    class Count : QueryStart
    {
        public override IQueryCompilerLevel1<TEntity> From<TEntity>()
        {
            return new SelectCountFrom<TEntity>(_Translator, _Breaker);
        }
    }
}
