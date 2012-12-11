using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq.Expressions;
using System.Text;
using VODB.DbLayer.DbCommands;
using VODB.DbLayer.DbExecuters;
using VODB.ExpressionParser;
using VODB.Extensions;
using VODB.VirtualDataBase;
using System.Linq;

namespace VODB.DbLayer.DbResults
{

    internal sealed class DbQueryResult<TEntity> : IDbCommandFactory,
                                                    IDbAndQueryResult<TEntity>,
                                                    IDbQueryResult<TEntity>,
                                                    IDbOrderedResult<TEntity>,
                                                    IDbOrderedDescResult<TEntity>,
                                                    IDbFieldFilterResult<TEntity>
        where TEntity : Entity, new()
    {

        private readonly IDbCommandFactory _CommandFactory;
        private readonly IQueryExecuter<TEntity> _Executer;
        private readonly StringBuilder _whereCondition;
        private readonly IWhereExpressionParser<TEntity> _ExpressionParser;
        private readonly ICollection<KeyValuePair<Key, Object>> _parameters;
        private Field _FilterField;

        #region IDbResult

        public String TableName { get { return new TEntity().Table.TableName; } }

        public String WhereCondition { get { return _whereCondition.ToString(); } }

        public IEnumerable<KeyValuePair<Key, Object>> Parameters
        {
            get
            {
                return _ExpressionParser.ConditionData.Union(_parameters);
            }
            set
            {
                foreach (var item in value)
                {
                    _parameters.Add(item);
                }
            }
        }

        #endregion

        public DbQueryResult(IDbCommandFactory commandFactory, IQueryExecuter<TEntity> executer)
        {
            _CommandFactory = commandFactory;
            _Executer = executer;
            _whereCondition = new StringBuilder();
            _ExpressionParser = new ComparatorExpressionParser<TEntity>();
            _parameters = new List<KeyValuePair<Key, Object>>();
        }

        public IEnumerator<TEntity> GetEnumerator()
        {
            return _Executer.GetEnumerator();

        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void SetParameters(DbCommand cmd, IEnumerable<KeyValuePair<Key, Object>> collection)
        {
            foreach (var data in collection)
            {
                cmd.SetParameter(data.Key.Field, data.Key.ParamName, data.Value);
            }
        }

        public DbCommand Make()
        {
            var cmd = _CommandFactory.Make();
            cmd.CommandText += _whereCondition.ToString();

            SetParameters(cmd, _ExpressionParser.ConditionData);
            SetParameters(cmd, _parameters);

            _ExpressionParser.ClearData();
            _whereCondition.Clear();

            return cmd;
        }

        #region IDbQueryResult

        public IDbAndQueryResult<TEntity> Where(string whereCondition, params object[] args)
        {
            _whereCondition.Append(" Where ").AppendFormat(whereCondition, args);
            return this;
        }

        public IDbAndQueryResult<TEntity> Where(Expression<Func<TEntity, bool>> whereCondition)
        {
            return Where(_ExpressionParser.Parse(whereCondition));
        }

        public IDbFieldFilterResult<TEntity> Where<TField>(Expression<Func<TEntity, TField>> field)
        {
            _whereCondition.Append(" Where ");
            var parser = new FieldGetterExpressionParser<TEntity, TField>();
            parser.Parse(field);
            _FilterField = parser.Field;
            return this;
        }

        #endregion

        #region IDbAndQueryResult

        public IDbAndQueryResult<TEntity> And(string andCondition, params object[] args)
        {
            _whereCondition.Append(" And ").AppendFormat(andCondition, args);
            return this;
        }

        public IDbFieldFilterResult<TEntity> And<TField>(Expression<Func<TEntity, TField>> field)
        {
            _whereCondition.Append(" And ");
            var parser = new FieldGetterExpressionParser<TEntity, TField>();
            parser.Parse(field);
            _FilterField = parser.Field;
            return this;
        }

        public IDbAndQueryResult<TEntity> And(Expression<Func<TEntity, bool>> andCondition)
        {
            return And(_ExpressionParser.Parse(andCondition));
        }

        #endregion

        #region OrderedResult

        public IDbOrderedResult<TEntity> OrderBy<TField>(Expression<Func<TEntity, TField>> orderByField)
        {
            _whereCondition.Append(" Order By ").Append(new FieldGetterExpressionParser<TEntity, TField>().Parse(orderByField));
            return this;
        }

        public IDbOrderedDescResult<TEntity> Descending()
        {
            _whereCondition.Append(" desc");
            return this;
        }

        #endregion

        #region IDbFieldFilterResult

        public IDbAndQueryResult<TEntity> In<TField>(IEnumerable<TField> args)
        {

            if (args is IDbResult)
            {
                return InResult((IDbResult)args);
            }

            _whereCondition
                .Append(_FilterField.FieldName)
                .Append(" in (");

            foreach (var arg in args)
            {
                _whereCondition
                    .Append(AddParameter(_FilterField, arg))
                    .Append(", ");
            }

            _whereCondition
                .Remove(_whereCondition.Length - 2, 2)
                .Append(") ");

            return this;
        }

        private IDbAndQueryResult<TEntity> InResult(IDbResult collection)
        {
            Parameters = collection.Parameters;
            _whereCondition
                .AppendFormat("{0} In (Select {1} from {2}", 
                    _FilterField.FieldName, 
                    _FilterField.BindedTo ?? _FilterField.FieldName, 
                    collection.TableName)
                .Append(collection.WhereCondition)
                .Append(")");

            return this;
        }

        public IDbAndQueryResult<TEntity> Between<TField>(TField firstValue, TField secondValue)
        {
            _whereCondition
                .Append(_FilterField.FieldName)
                .AppendFormat(" Between {0} And {1}", 
                    AddParameter(_FilterField, firstValue), 
                    AddParameter(_FilterField, secondValue));

            return this;
        }

        public IDbAndQueryResult<TEntity> Like(String value, WildCard token = WildCard.Both)
        {
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

            return this;
        }

        #endregion

        private String AddParameter(Field field, Object value)
        {
            var paramName = String.Format("@{0}{1}{2}", field.FieldName, Environment.TickCount, _parameters.Count);
            _parameters.Add(new KeyValuePair<Key, Object>(new Key(field, paramName), value));
            return paramName;
        }

        
    }

}
