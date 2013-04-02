using System.Collections.Generic;
using System;
using System.Linq;
using VODB.Core.Execution.Executers.DbResults;
using System.Linq.Expressions;
using System.Collections;
using VODB.ExpressionsToSql;
using VODB.EntityTranslation;
using VODB.DbLayer;

namespace VODB.QueryCompiler
{

    class QueryCompiler<TEntity> : IQueryCompilerLevel1<TEntity>, IQueryCompilerLevel2<TEntity>, IQueryCompilerLevel3<TEntity>, IQueryCompilerLevel4<TEntity>, IQueryCompilerStub<TEntity>
        where TEntity : class, new()
    {
        private static IQueryCondition _Right_parenthesis = new ConstantCondition(")");
        private static IQueryCondition _Left_parenthesis = new ConstantCondition("(");
        private static IQueryCondition _Or_Condition = new ConstantCondition(" Or ");
        private static IQueryCondition _And_Condition = new ConstantCondition(" And ");

        /// <summary>
        /// Holds the conditions and enables compilation.
        /// </summary>
        private IQueryConditionComposite _Query = new QueryCondition();

        private IEntityTranslator _Translator;
        private Expression<Func<TEntity, object>> _PartialExpression;
        private IQueryCondition _LastCondition = null;
        private bool _WasLastOr;
        private String _CompiledQuery = null;
        private int _ConditionCount = -1;
        private IInternalSession _Session;

        public IVodbCommand CachedCommand { get; set; }

        public QueryCompiler(IEntityTranslator translator, Func<IQueryCompilerLevel1<TEntity>, IEnumerable<TEntity>> func)
            : this(translator)
        {
            _Translator = translator;
            func(this);
        }

        public QueryCompiler(IEntityTranslator translator)
        {
            _Translator = translator;
        }

        public QueryCompiler(IEntityTranslator translator, IInternalSession session)
            : this(translator)
        {
            _Session = session;
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

        private QueryCompiler<TEntity> Add(String condition)
        {
            _Query.Add(new ConstantCondition(condition));
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
            return Add(" Where ").Add(expression);
        }

        public IQueryCompilerLevel2<TEntity> Where(string mask, params object[] args)
        {
            return Add(" Where ").Add(String.Format(mask, args));
        }

        public IQueryCompilerLevel4<TEntity> Where(Expression<Func<TEntity, object>> expression)
        {
            Add(" Where ");
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
            Add(_And_Condition);
            return Add(expression);
        }

        public IQueryCompilerLevel2<TEntity> And(string mask, params object[] args)
        {
            return Add(_And_Condition)
                .Add(String.Format(mask, args));
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

            Add(_Or_Condition);
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

            Add(_Or_Condition);
            _PartialExpression = expression;

            _LastCondition = _Right_parenthesis;

            _WasLastOr = true;
            return this;
        }

        #endregion

        #region IQueryCompilerLelvel3<TEntity> Implementation

        public IQueryCompilerStub<TEntity> Descending()
        {
            return Add(" Desc");
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
            IQueryCondition inCondition = new InCondition(collection);

            if (collection is IQuery)
            {
                inCondition = ((IQuery)collection).WhereCompile;
            }

            return Add(new QueryCondition<TEntity>(_Translator, _PartialExpression, inCondition))
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
            if (_Session == null)
            {
                throw new InvalidOperationException("The QueryCompiler has no session.");
            }

            return Execute(_Session).GetEnumerator();
        } 

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region IQuery<TEntity> Implementation

        public IEnumerable<TEntity> Execute(ISession session)
        {
            return _Session.ExecuteQuery<TEntity>(this, Parameters.Select(p => p.Value).ToArray());
        }

        public string Compile(ref int level)
        {
            if (_ConditionCount == _Query.Count())
            {
                return _CompiledQuery;
            }

            CachedCommand = null;
            _ConditionCount = _Query.Count();
            return _CompiledQuery = _Translator.Translate(typeof(TEntity)).SqlSelect + _Query.Compile(ref level);
        }

        public IEnumerable<IQueryParameter> Parameters
        {
            get { return _Query.Parameters; }
        }

        #endregion
        
        public int Count()
        {
            if (_Session == null)
            {
                throw new InvalidOperationException("The QueryCompiler has no session.");
            }

            int level = 0;
            string sql = _Translator.Translate(typeof(TEntity)).SqlCount + _Query.Compile(ref level);
            return (int)_Session.ExecuteScalar(sql, Parameters.ToArray());
        }

        public IQueryCondition WhereCompile
        {
            get { return _Query; }
        }
    }
}
