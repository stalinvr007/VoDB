using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VODB.Core.Execution.Executers.DbResults;
using VODB.DbLayer;
using VODB.EntityTranslation;
using VODB.Exceptions;
using VODB.Expressions;
using VODB.ExpressionsToSql;
using VODB.Infrastructure;
using VODB.QueryCompiler.ExpressionPiecesToSql;

namespace VODB.QueryCompiler
{

    abstract class QueryBase<TEntity> : IQueryCompilerLevel1<TEntity>, IQueryCompilerLevel2<TEntity>, IQueryCompilerLevel3<TEntity>, IQueryCompilerLevel4<TEntity>, IQueryCompilerStub<TEntity>
        where TEntity : class, new()
    {

        private readonly IEntityTranslator _Translator;
        private IInternalSession _Session;
        private ICollection<IQueryParameter> _Parameters = new List<IQueryParameter>();
        private IExpressionBreaker _Breaker;
        private CompositeCompiler _Composite;
        private ISqlCompiler _Parameter;

        private bool calledFromOr = false;
        private int level = 1;
        private int lastIndex = 0;

        private IEnumerable<IExpressionPiece> _Pieces;
        private static ISqlCompiler _Where = new ConstantCompiler(" Where ");
        private static ISqlCompiler _OrderBy = new ConstantCompiler(" Order By ");
        private static ISqlCompiler _And = new ConstantCompiler(" And ");
        private static ISqlCompiler _Or = new ConstantCompiler(" Or ");
        private static ISqlCompiler _Stub = new ConstantCompiler("");
        private static ISqlCompiler _ParenthesisClose = new ConstantCompiler(")");
        private static ISqlCompiler _ParenthesisOpen = new ConstantCompiler("(");

        public ITable Table { get; private set; }

        public QueryBase(IEntityTranslator translator, IExpressionBreaker breaker)
        {
            _Composite = new CompositeCompiler();
            _Translator = translator;
            _Breaker = breaker;
            _Parameter = new ParameterCompiler(GetNumber);
            Table = _Translator.Translate(typeof(TEntity));
        }

        private int paramCount = 0;
        private int GetNumber()
        {
            return ++paramCount;
        }

        #region IQueryCompilerLevel1<TEntity> Implementation

        public IQueryCompilerLevel2<TEntity> Where(Expression<Func<TEntity, bool>> expression)
        {
            _Composite.Add(_Where);
            _Composite.Add(
                new PiecesCompiler(
                    _Breaker.BreakExpression(expression),
                    new ParameterCompiler(GetNumber, expression.Body.NodeType))
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
            _Composite.Add(_And);
            _Composite.Add(
                new PiecesCompiler(
                    _Breaker.BreakExpression(expression),
                    new ParameterCompiler(GetNumber, expression.Body.NodeType))
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

            _Composite.Add(_Or);
            _Composite.Add(
                new PiecesCompiler(
                    _Breaker.BreakExpression(expression),
                    new ParameterCompiler(GetNumber, expression.Body.NodeType))
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
                new PiecesCompiler(_Pieces, new LikeCompiler(_Parameter, token))
            );

            return this;
        }

        public IQueryCompilerLevel2<TEntity> In<TField>(IEnumerable<TField> collection)
        {

            if (collection is IQuery)
            {
                IQuery query = ((IQuery)collection);
                _Composite.Add(new SubQueryCompiler(query.Table, _Pieces, query.SqlCompiler));
            }
            else
            {
                _Composite.Add(new PiecesCompiler(_Pieces, new InStatementCompiler(collection.Select(f => _Parameter))));
            }
            
            return this;
        }

        public IQueryCompilerLevel2<TEntity> Between<TField>(TField firstValue, TField secondValue)
        {
            _Composite.Add(
                new PiecesCompiler(_Pieces, new BetweenCompiler(_Parameter, _Parameter))
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
            return this.GetEnumerator();
        }

        #endregion

        #region IQuery<TEntity> Implementation

        public IEnumerable<TEntity> Execute(ISession session)
        {
            return _Session.ExecuteQuery<TEntity>(this, Parameters.Select(p => p.Value).ToArray());
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
            throw new NotImplementedException();
        }

        protected abstract string Compile(ITable table);

    }

    class SelectAllFrom<TEntity> : QueryBase<TEntity> where TEntity : class, new()
    {
        public SelectAllFrom(IEntityTranslator translator, IExpressionBreaker breaker)
            : base(translator, breaker)
        {

        }

        protected override string Compile(ITable table)
        {
            return table.SqlSelect;
        }
    }

    class SelectCountFrom<TEntity> : QueryBase<TEntity> where TEntity : class, new()
    {
        public SelectCountFrom(IEntityTranslator translator, IExpressionBreaker breaker)
            : base(translator, breaker)
        {

        }

        protected override string Compile(ITable table)
        {
            return table.SqlCount;
        }
    }

}
