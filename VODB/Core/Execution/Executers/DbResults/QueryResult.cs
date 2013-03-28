using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using VODB.Core.Infrastructure;
using VODB.Core.Loaders;
using VODB.Core.Loaders.Factories;
using VODB.ExpressionParser;
using VODB.QueryCompiler;

namespace VODB.Core.Execution.Executers.DbResults
{
    internal interface IQueryResultGetter
    {
        IDbQueryResult<TEntity> GetQueryResult<TEntity>(IInternalSession session, IEntityLoader loader, IEntityFactory entityFactory) where TEntity : class, new();
    }

    internal class QueryResultGetter : IQueryResultGetter
    {
        #region IQueryResultGetter Members

        public IDbQueryResult<TEntity> GetQueryResult<TEntity>(IInternalSession session, IEntityLoader loader, IEntityFactory entityFactory)
            where TEntity : class, new()
        {
            return new QueryResult<TEntity>(session, loader, entityFactory);
        }

        #endregion
    }

    internal class QueryResult<TEntity> : IDbQueryResult<TEntity>, IDbAndQueryResult<TEntity>,
                                          IDbFieldFilterResult<TEntity>, IDbOrderedResult<TEntity>,
                                          IDbOrderedDescResult<TEntity>,
                                          IQueryCompilerLevel1<TEntity>,
                                          IQueryCompilerLevel2<TEntity>,
                                          IQueryCompilerLevel3<TEntity>,
                                          IQueryCompilerLevel4<TEntity>,
                                          IQueryCompilerStub<TEntity>
        where TEntity : class, new()
    {
        private readonly IEntityFactory _EntityFactory;
        private readonly IWhereExpressionParser<TEntity> _ExpressionParser;
        private readonly IEntityLoader _Loader;
        private readonly ICollection<KeyValuePair<Key, Object>> _Parameters;
        private readonly LinkedList<ConditionPart> _Parts;
        private readonly IInternalSession _Session;
        private readonly Table _Table;
        private Field _FilterField;

        public QueryResult(IInternalSession session, IEntityLoader loader, IEntityFactory entityFactory)
        {
            _EntityFactory = entityFactory;
            _Loader = loader;
            _Session = session;
            _Table = Engine.GetTable<TEntity>();
            _ExpressionParser = new ComparatorExpressionParser<TEntity>();
            _Parameters = new List<KeyValuePair<Key, Object>>();
            _Parts = new LinkedList<ConditionPart>();
        }

        #region Auxiliary Methods

        private QueryResult<TEntity> AddCondition(Operation operation, String condition)
        {
            _Parts.AddLast(new ConditionPart
                               {
                                   Operation = operation,
                                   Condition = condition
                               });
            return this;
        }

        private static String BuildWhereCondition(LinkedList<ConditionPart> parts)
        {
            var sql = new StringBuilder();

            foreach (ConditionPart current in parts)
            {
                string operation = current.Operation.GetString();

                if (operation != null)
                {
                    sql.Append(operation);
                }
                sql.Append(current.Condition);
            }

            return sql.ToString();
        }

        private void AppendToConditions(StringBuilder builder)
        {
            _Parts.Last.Value.Condition = FinishOrCondition(builder).ToString();
        }

        private StringBuilder FinishOrCondition(StringBuilder builder)
        {
            if (_Parts.Last.Value.Operation == Operation.Or)
            {
                builder.Append(")");
            }
            return builder;
        }

        private String AddParameter(Field field, Object value)
        {
            string paramName = String.Format("@{0}db{1}", field.FieldName, _Parameters.Count);
            _Parameters.Add(new KeyValuePair<Key, Object>(new Key(field, paramName), value));
            return paramName;
        }

        #endregion

        #region IDbQueryResult Implementation

        IQueryCompilerLevel2<TEntity> IQueryCompilerLevel1<TEntity>.Where(Expression<Func<TEntity, bool>> expression)
        {
            return Where(expression);
        }

        IQueryCompilerLevel2<TEntity> IQueryCompilerLevel1<TEntity>.Where(string mask, params object[] args)
        {
            return Where(mask, args);
        }

        public IQueryCompilerLevel4<TEntity> Where(Expression<Func<TEntity, object>> expression)
        {
            return Where<Object>(expression);
        }

        public IQueryCompilerLevel3<TEntity> OrderBy(Expression<Func<TEntity, object>> expression)
        {
            return OrderBy<Object>(expression);
        }

        public IEnumerable<TEntity> Execute(ISession session)
        {
            throw new NotImplementedException();
        }

        public string Compile(ref int level)
        {
            throw new NotImplementedException();
        }

        IEnumerable<ExpressionsToSql.IQueryParameter> ExpressionsToSql.IQueryCondition.Parameters
        {
            get { throw new NotImplementedException(); }
        }

        public IDbAndQueryResult<TEntity> Where(string whereCondition, params object[] args)
        {
            return AddCondition(Operation.Where, String.Format(whereCondition, args));
        }

        public IDbAndQueryResult<TEntity> Where(Expression<Func<TEntity, bool>> whereCondition)
        {
            return AddCondition(Operation.Where, _ExpressionParser.Parse(whereCondition));
        }

        public IDbFieldFilterResult<TEntity> Where<TField>(Expression<Func<TEntity, TField>> field)
        {
            var parser = new FieldGetterExpressionParser<TEntity, TField>();
            parser.Parse(field);
            _FilterField = parser.Field;
            return AddCondition(Operation.Where, "");
        }

        public IDbOrderedResult<TEntity> OrderBy<TField>(Expression<Func<TEntity, TField>> orderByField)
        {
            return AddCondition(Operation.OrderBy,
                                new FieldGetterExpressionParser<TEntity, TField>().Parse(orderByField));
        }

        public IEnumerator<TEntity> GetEnumerator()
        {
            _Session.Open();
            DbCommand cmd = _Session.CreateCommand();
            cmd.CommandText = _Table.CommandsHolder.Select + WhereCondition;

            cmd.SetParameters(_ExpressionParser.ConditionData);
            cmd.SetParameters(_Parameters);

            IDataReader reader = cmd.ExecuteReader();
            try
            {
                var list = new List<TEntity>();
                while (reader.Read())
                {
                    var newTEntity = _EntityFactory.Make<TEntity>(_Session);
                    _Loader.Load(newTEntity, _Session, reader);
                    list.Add(newTEntity);
                }
                return list.GetEnumerator();
            }
            finally
            {
                reader.Close();
                _Session.Close();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerable<KeyValuePair<Key, Object>> Parameters
        {
            get { return _ExpressionParser.ConditionData.Union(_Parameters); }
            set
            {
                foreach (var item in value)
                {
                    _Parameters.Add(item);
                }
            }
        }

        public string TableName
        {
            get { return _Table.TableName; }
        }

        public String WhereCondition
        {
            get { return BuildWhereCondition(_Parts); }
        }

        #endregion

        #region IDbAndQueryResult Implementation

        IQueryCompilerLevel2<TEntity> IQueryCompilerLevel2<TEntity>.And(Expression<Func<TEntity, bool>> expression)
        {
            return And(expression);
        }

        IQueryCompilerLevel2<TEntity> IQueryCompilerLevel2<TEntity>.And(string mask, params object[] args)
        {
            return And(mask, args);
        }

        public IQueryCompilerLevel4<TEntity> And(Expression<Func<TEntity, object>> expression)
        {
            return And<Object>(expression);
        }

        IQueryCompilerLevel2<TEntity> IQueryCompilerLevel2<TEntity>.Or(Expression<Func<TEntity, bool>> expression)
        {
            return Or(expression);
        }

        public IQueryCompilerLevel4<TEntity> Or(Expression<Func<TEntity, object>> expression)
        {
            return Or<Object>(expression);
        }

        public IDbAndQueryResult<TEntity> And(string andCondition, params object[] args)
        {
            return AddCondition(Operation.And, String.Format(andCondition, args));
        }

        public IDbFieldFilterResult<TEntity> And<TField>(Expression<Func<TEntity, TField>> field)
        {
            var parser = new FieldGetterExpressionParser<TEntity, TField>();
            parser.Parse(field);
            _FilterField = parser.Field;
            return AddCondition(Operation.And, "");
        }

        public IDbAndQueryResult<TEntity> And(Expression<Func<TEntity, bool>> andCondition)
        {
            return AddCondition(Operation.And, _ExpressionParser.Parse(andCondition));
        }

        public IDbFieldFilterResult<TEntity> Or<TField>(Expression<Func<TEntity, TField>> field)
        {
            var parser = new FieldGetterExpressionParser<TEntity, TField>();
            parser.Parse(field);
            _FilterField = parser.Field;

            _Parts.Last.Value.Condition = "(" + _Parts.Last.Value.Condition;
            return AddCondition(Operation.Or, "");
        }

        public IDbAndQueryResult<TEntity> Or(Expression<Func<TEntity, bool>> orCondition)
        {
            _Parts.Last.Value.Condition = "(" + _Parts.Last.Value.Condition;

            return AddCondition(Operation.Or, _ExpressionParser.Parse(orCondition) + ")");
        }

        #endregion

        #region IDbFieldFilterResult<TEntity> Implementation

        IQueryCompilerLevel2<TEntity> IQueryCompilerLevel4<TEntity>.Like(string value, WildCard token = WildCard.Both)
        {
            return Like(value, token);
        }

        public IQueryCompilerLevel2<TEntity> In(IEnumerable<object> collection)
        {
            return In<Object>(collection);
        }

        public IQueryCompilerLevel2<TEntity> Between(object firstValue, object secondValue)
        {
            return Between<Object>(firstValue, secondValue);
        }

        public IDbAndQueryResult<TEntity> In<TField>(IEnumerable<TField> args)
        {
            if (args is IDbResult)
            {
                return InResult((IDbResult) args);
            }
            var _whereCondition = new StringBuilder();

            _whereCondition
                .Append(_FilterField.FieldName)
                .Append(" In (");

            foreach (TField arg in args)
            {
                _whereCondition
                    .Append(AddParameter(_FilterField, arg))
                    .Append(", ");
            }

            _whereCondition
                .Remove(_whereCondition.Length - 2, 2)
                .Append(") ");

            AppendToConditions(_whereCondition);
            return this;
        }

        public IDbAndQueryResult<TEntity> Between<TField>(TField firstValue, TField secondValue)
        {
            var _whereCondition = new StringBuilder();

            _whereCondition
                .Append(_FilterField.FieldName)
                .AppendFormat(" Between {0} And {1}",
                              AddParameter(_FilterField, firstValue),
                              AddParameter(_FilterField, secondValue));

            AppendToConditions(_whereCondition);
            return this;
        }

        public IDbAndQueryResult<TEntity> Like(String value, WildCard token = WildCard.Both)
        {
            var _whereCondition = new StringBuilder();
            _whereCondition
                .Append(_FilterField.FieldName)
                .Append(" Like '");

            if (token == WildCard.Left || token == WildCard.Both)
            {
                _whereCondition.Append("%");
            }

            _whereCondition.Append(value);

            if (token == WildCard.Right || token == WildCard.Both)
            {
                _whereCondition.Append("%");
            }

            _whereCondition.Append("'");

            AppendToConditions(_whereCondition);
            return this;
        }

        private IDbAndQueryResult<TEntity> InResult(IDbResult collection)
        {
            var _whereCondition = new StringBuilder();

            Parameters = collection.Parameters;
            _whereCondition
                .AppendFormat("{0} In (Select {1} from {2}",
                              _FilterField.FieldName,
                              _FilterField.BindedTo ?? _FilterField.FieldName,
                              collection.TableName)
                .Append(collection.WhereCondition)
                .Append(")");

            AppendToConditions(_whereCondition);
            return this;
        }

        #endregion

        #region IDbOrderedResult<TEntity> Implementation

        public IDbOrderedDescResult<TEntity> Descending()
        {
            return AddCondition(Operation.Desc, "");
        }

        #endregion


        IQueryCompilerStub<TEntity> IQueryCompilerLevel3<TEntity>.Descending()
        {
            return Descending();
        }

        
    }
}