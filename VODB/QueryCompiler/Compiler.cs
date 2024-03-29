﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using VODB.Core.Execution.Executers.DbResults;
using VODB.DbLayer;
using VODB.EntityTranslation;
using VODB.Expressions;
using VODB.ExpressionsToSql;
using VODB.Infrastructure;
using VODB.QueryCompiler.ExpressionPiecesToSql;

namespace VODB.QueryCompiler
{

    abstract class QueryBase<TEntity> :IQuery, IQueryCompilerLevel1<TEntity>, IQueryCompilerLevel2<TEntity>, IQueryCompilerLevel3<TEntity>, IQueryCompilerLevel4<TEntity>, IQueryCompilerStub<TEntity>
        where TEntity : class, new()
    {

        private readonly IEntityTranslator _Translator;
        private readonly IInternalSession _Session;
        private readonly ICollection<IQueryParameter> _Parameters = new List<IQueryParameter>();
        private readonly IExpressionBreaker _Breaker;
        private readonly CompositeCompiler _Composite;

        private bool calledFromOr;

        private IEnumerable<IExpressionPiece> _Pieces;
        // ReSharper disable StaticFieldInGenericType
        private static readonly ISqlCompiler _Where = new ConstantCompiler(" Where ");
        private static readonly ISqlCompiler _OrderBy = new ConstantCompiler(" Order By ");
        private static readonly ISqlCompiler _And = new ConstantCompiler(" And ");
        private static readonly ISqlCompiler _Or = new ConstantCompiler(" Or ");
        private static readonly ISqlCompiler _ParenthesisClose = new ConstantCompiler(")");
        private static readonly ISqlCompiler _ParenthesisOpen = new ConstantCompiler("(");
        // ReSharper restore StaticFieldInGenericType

        public ITable Table { get; private set; }

        protected QueryBase(IEntityTranslator translator, IExpressionBreaker breaker, IInternalSession session)
        {
            _Composite = new CompositeCompiler();
            _Translator = translator;
            _Session = session;
            _Breaker = breaker;
            Table = _Translator.Translate(typeof(TEntity));

            AddParameter = (f, v) => _Parameters.Add(++paramCount, f, v);
        }

        protected QueryBase(IEntityTranslator translator, IExpressionBreaker breaker, IInternalSession session, ISqlCompiler conditions, IEnumerable<IQueryParameter> parameters)
            : this(translator, breaker, session)
        {
            _Composite.Add(conditions);
            foreach (var parameter in parameters)
            {
                _Parameters.Add(parameter);
            }
        } 

        public Func<IField, Object, String> AddParameter { get; set; }
        private int paramCount;


        private String AddParam(IField field, Object value)
        {
            return AddParameter(field, value);
        }


        #region IQueryCompilerLevel1<TEntity> Implementation

        public IQueryCompilerLevel2<TEntity> Where(Expression<Func<TEntity, bool>> expression)
        {
            var pieces = _Breaker.BreakExpression(expression).ToList();

            _Composite.Add(_Where);
            _Composite.Add(
                new PiecesCompiler(
                    pieces,
                    new ParameterCompiler(v => AddParam(pieces.Last().Field, v), expression.GetRightValue(), expression.Body.NodeType))
            );
            
            return this;
        }

        public IQueryCompilerLevel2<TEntity> Where(string mask, params object[] args)
        {
            _Composite.Add(_Where);
            _Composite.Add(
                new ConstantCompiler(mask, args)
            );
            return this;
        }

        public IQueryCompilerLevel4<TEntity> Where<TField>(Expression<Func<TEntity, TField>> expression)
        {
            _Composite.Add(_Where);
            _Pieces = _Breaker.BreakExpression(expression);
            return this;
        }

        public IQueryCompilerLevel3<TEntity> OrderBy<TField>(Expression<Func<TEntity, TField>> expression)
        {
            CloseParenthesis();
            _Composite.Add(_OrderBy);
            _Composite.Add(
                new OrderByCompiler(_Breaker.BreakExpression(expression))
            );
            return this;
        }

        #endregion

        #region IQueryCompilerLelvel2<TEntity> Implementation

        public IQueryCompilerLevel2<TEntity> And(Expression<Func<TEntity, bool>> expression)
        {
            CloseParenthesis();
            var pieces = _Breaker.BreakExpression(expression).ToList();

            _Composite.Add(_And);
            _Composite.Add(
                new PiecesCompiler(
                    pieces,
                    new ParameterCompiler(v => AddParam(pieces.Last().Field, v), expression.GetRightValue(), expression.Body.NodeType))
            );

            return this;
        }

        public IQueryCompilerLevel2<TEntity> And(string mask, params object[] args)
        {
            CloseParenthesis();
            _Composite.Add(_And);
            _Composite.Add(
                new ConstantCompiler(mask, args)
            );
            return this;
        }

        public IQueryCompilerLevel4<TEntity> And<TField>(Expression<Func<TEntity, TField>> expression)
        {
            CloseParenthesis();
            _Composite.Add(_And);            
            _Pieces = _Breaker.BreakExpression(expression);
            return this;
        }

        private void OpenParenthesis()
        {
            if (!calledFromOr)
            {
                _Composite.Insert(_Composite.Count - 1, _ParenthesisOpen);
            }
        }

        private void CloseParenthesis()
        {
            if (calledFromOr)
            {
                _Composite.Add(_ParenthesisClose);
                calledFromOr = false;
            }
        }

        public IQueryCompilerLevel2<TEntity> Or(Expression<Func<TEntity, bool>> expression)
        {
            OpenParenthesis();

            var pieces = _Breaker.BreakExpression(expression).ToList();

            _Composite.Add(_Or);
            _Composite.Add(
                new PiecesCompiler(
                    pieces,
                    new ParameterCompiler(v => AddParam(pieces.Last().Field, v), expression.GetRightValue(), expression.Body.NodeType))
            );

            calledFromOr = true;
            return this;
        }

        public IQueryCompilerLevel4<TEntity> Or<TField>(Expression<Func<TEntity, TField>> expression)
        {
            OpenParenthesis();

            _Composite.Add(_Or);
            _Pieces = _Breaker.BreakExpression(expression);
            calledFromOr = true;
            return this;
        }

        #endregion

        #region IQueryCompilerLelvel3<TEntity> Implementation

        public IQueryCompilerStub<TEntity> Descending()
        {
            
            _Composite.Add(new ConstantCompiler(" Desc"));
            return this;
        }

        #endregion

        #region IQueryCompilerLevel4<TEntity> Implementation

        public IQueryCompilerLevel2<TEntity> Like(string value, WildCard token = WildCard.Both)
        {
            _Composite.Add(
                new PiecesCompiler(_Pieces, new LikeCompiler(
                    new ParameterCompiler(v => AddParam(_Pieces.Last().Field, v), value, ExpressionType.Parameter), token))
            );
            
            return this;
        }

        public IQueryCompilerLevel2<TEntity> In<TField>(IEnumerable<TField> collection)
        {

            if (collection is IQuery)
            {
                var query = ((IQuery)collection);
                query.AddParameter = AddParameter;
                _Composite.Add(new SubQueryCompiler(query.Table, _Pieces, query.SqlCompiler));
            }
            else
            {
                _Composite.Add(new PiecesCompiler(_Pieces, 
                    new InStatementCompiler(collection.Select(f => new ParameterCompiler(v => AddParam(_Pieces.Last().Field, v), f, ExpressionType.Parameter))))
                );
            }
            
            return this;
        }

        public IQueryCompilerLevel2<TEntity> Between<TField>(TField firstValue, TField secondValue)
        {
            _Composite.Add(
                new PiecesCompiler(_Pieces, new BetweenCompiler(
                    new ParameterCompiler(v => AddParam(_Pieces.Last().Field, v), firstValue, ExpressionType.Parameter),
                    new ParameterCompiler(v => AddParam(_Pieces.Last().Field, v), secondValue, ExpressionType.Parameter)))
            );
            
            return this;
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
            return GetEnumerator();
        }

        #endregion

        #region IQuery<TEntity> Implementation

        public IEnumerable<TEntity> Execute(ISession session)
        {
            return _Session.InternalExecuteQuery(this, Parameters.Select(p => p.Value).ToArray());
        }

        public string Compile()
        {
            CloseParenthesis();
            return Compile(_Translator.Translate(typeof(TEntity))) + _Composite.Compile();
        }

        public IEnumerable<IQueryParameter> Parameters
        {
            get { return _Parameters; }
        }

        #endregion

        public IVodbCommand CachedCommand { get; set; }

        public IQueryCondition WhereCompile
        {
            get { throw new NotImplementedException(); }
        }

        public ISqlCompiler SqlCompiler { get { CloseParenthesis(); return _Composite; } }

        public int Count()
        {
            CloseParenthesis();
            return (int)_Session.ExecuteScalar(
                _Translator.Translate(typeof(TEntity)).SqlCount + _Composite.Compile(), 
                Parameters);
        }

        protected abstract string Compile(ITable table);

        public object InternalWhere(IField field, IQueryParameter parameter)
        {
            _Composite.Add(_Composite.Count == 0 ? _Where : _And);
            _Composite.Add(new ConstantCompiler("{0}", field.BindOrName));
            _Composite.Add(new ParameterCompiler(v => AddParam(field, v), parameter.Value, ExpressionType.Equal));
            return this;
        }
    }

    class SelectAllFrom<TEntity> : QueryBase<TEntity> where TEntity : class, new()
    {
        public SelectAllFrom(IEntityTranslator translator, IExpressionBreaker breaker, IInternalSession session)
            : base(translator, breaker, session) { }

        public SelectAllFrom(IEntityTranslator translator, IExpressionBreaker breaker, IInternalSession session, ISqlCompiler conditions, IEnumerable<IQueryParameter> parameters)
            : base(translator, breaker, session, conditions, parameters) { }

        protected override string Compile(ITable table)
        {
            return table.SqlSelect;
        }
    }

}
