using System.Collections.Generic;
using System;
using VODB.Core.Execution.Executers.DbResults;
using System.Linq.Expressions;
using System.Collections;
using VODB.ExpressionsToSql;
using VODB.EntityTranslation;

namespace VODB.QueryCompiler
{

    class QueryCompiler<TEntity> : IQueryCompilerLevel1<TEntity>, IQueryCompilerLevel2<TEntity>, IQueryCompilerLevel3<TEntity>, IQueryCompilerLevel4<TEntity>, IQueryCompilerStub<TEntity>
    {
        private static IQueryCondition _Right_parenthesis = new ConstantCondition(")");
        private static IQueryCondition _Left_parenthesis = new ConstantCondition("(");

        /// <summary>
        /// Holds the conditions and enables compilation.
        /// </summary>
        private IQueryConditionComposite _Query = new QueryCondition();
        
        private IEntityTranslator _Translator;
        private Expression<Func<TEntity, object>> _PartialExpression;
        private IQueryCondition _LastCondition = null;
        private bool _WasLastOr;

        public QueryCompiler(IEntityTranslator translator, Func<IQueryCompilerLevel1<TEntity>, IEnumerable<TEntity>> func)
        {
            _Translator = translator;
            func(this);
        }

        private IQueryCompilerLevel2<TEntity> AppendLastCondition()
        {
            if (_LastCondition == null)
            {
                return this;
            }

            Add(_LastCondition);
            _LastCondition = null;
            return this;
        }

        private QueryCompiler<TEntity> Add(IQueryCondition condition)
        {
            _Query.Add(condition);
            return this;
        }

        private QueryCompiler<TEntity> Add(Expression<Func<TEntity, bool>> expression)
        {
            _Query.Add(new QueryCondition<TEntity>(_Translator, expression));
            return this;
        }
                
        #region IQueryCompilerLevel1<TEntity> Implementation

        public IQueryCompilerLevel2<TEntity> Where(Expression<Func<TEntity, bool>> expression)
        {
            return Add(expression);
        }

        public IQueryCompilerLevel4<TEntity> Where(Expression<Func<TEntity, object>> expression)
        {
            _PartialExpression = expression;
            return this;
        }

        public IQueryCompilerLevel3<TEntity> OrderBy(Expression<Func<TEntity, Object>> expression)
        {
            return Add(new OrderByCondition<TEntity>(_Translator, expression));
        }

        #endregion

        #region IQueryCompilerLelvel2<TEntity> Implementation

        public IQueryCompilerLevel2<TEntity> And(Expression<Func<TEntity, bool>> expression)
        {
            _WasLastOr = false;
            Add(new ConstantCondition(" And "));
            return Add(expression);
        }

        public IQueryCompilerLevel4<TEntity> And(Expression<Func<TEntity, object>> expression)
        {
            _WasLastOr = false;
            _PartialExpression = expression;
            return this;
        }

        public IQueryCompilerLevel2<TEntity> Or(Expression<Func<TEntity, bool>> expression)
        {
            if (!_WasLastOr)
            {
                _Query.InsertBeforeLast(_Left_parenthesis);
            }
            else
            {
                _Query.RemoveLast();
            }

            Add(new ConstantCondition(" Or "));
            Add(expression);

            Add(_Right_parenthesis);            

            _WasLastOr = true;
            return this;
        }

        public IQueryCompilerLevel4<TEntity> Or(Expression<Func<TEntity, object>> expression)
        {
            if (!_WasLastOr)
            {
                _Query.InsertBeforeLast(_Left_parenthesis);
            }
            else
            {
                _Query.RemoveLast();
            }

            Add(new ConstantCondition(" Or "));
            _PartialExpression = expression;

            _LastCondition = _Right_parenthesis;
            
            _WasLastOr = true;
            return this;
        }

        #endregion

        #region IQueryCompilerLelvel3<TEntity> Implementation

        public IQueryCompilerStub<TEntity> Descending()
        {
            return Add(new ConstantCondition(" Desc"));
        }

        #endregion

        #region IQueryCompilerLevel4<TEntity> Implementation

        public IQueryCompilerLevel2<TEntity> Like(string value, WildCard token = WildCard.Both)
        {
            return Add(new QueryCondition<TEntity>(_Translator, _PartialExpression, new LikeCondition(value, token)))
                .AppendLastCondition();
        }

        public IQueryCompilerLevel2<TEntity> In(IEnumerable<Object> collection)
        {
            return Add(new QueryCondition<TEntity>(_Translator, _PartialExpression, new InCondition(collection)))
                .AppendLastCondition();
        }

        public IQueryCompilerLevel2<TEntity> Between(Object firstValue, Object secondValue)
        {
            return Add(new QueryCondition<TEntity>(_Translator, _PartialExpression, new BetweenCondition(firstValue, secondValue)))
                .AppendLastCondition();
        }

        #endregion

        #region IEnumerable<TEntity> Implementation

        public IEnumerator<TEntity> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IQuery<TEntity> Implementation

        public IEnumerable<TEntity> Execute(ISession session)
        {
            throw new System.NotImplementedException();
        }

        public string Compile(ref int level)
        {
            return _Translator.Translate(typeof(TEntity)).SqlSelect + " Where " + _Query.Compile(ref level);
        }

        public IEnumerable<IQueryParameter> Parameters
        {
            get
            {
                return _Query.Parameters;
            }
        }

        #endregion

    }
}
