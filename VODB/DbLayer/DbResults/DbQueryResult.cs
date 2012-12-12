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
using System.Collections;

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
        private readonly IWhereExpressionParser<TEntity> _ExpressionParser;
        private readonly ICollection<KeyValuePair<Key, Object>> _parameters;
        private Field _FilterField;
        private readonly LinkedList<ConditionPart> _parts;
        
        #region IDbResult

        public String TableName { get { return new TEntity().Table.TableName; } }

        public String WhereCondition { get { return BuildWhereCondition(_parts); } }

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
            _ExpressionParser = new ComparatorExpressionParser<TEntity>();
            _parameters = new List<KeyValuePair<Key, Object>>();
            _parts = new LinkedList<ConditionPart>();
        }

        public IEnumerator<TEntity> GetEnumerator()
        {
            return _Executer.GetEnumerator();

        }

        IEnumerator IEnumerable.GetEnumerator()
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

        private static String BuildWhereCondition(LinkedList<ConditionPart> parts)
        {
            var sql = new StringBuilder();

            foreach (var current in parts)
            {
                var operation = current.Operation.GetString();

                if (operation != null)
                {
                    sql.Append(operation);
                }
                sql.Append(current.Condition);
            }

            return sql.ToString();
        }

        public DbCommand Make()
        {
            var cmd = _CommandFactory.Make();
            cmd.CommandText += WhereCondition;

            SetParameters(cmd, _ExpressionParser.ConditionData);
            SetParameters(cmd, _parameters);

            _ExpressionParser.ClearData();
            _parts.Clear();

            return cmd;
        }

        private DbQueryResult<TEntity> AddCondition(Operation operation, String condition)
        {
            _parts.AddLast(new ConditionPart
            {
                Operation = operation,
                Condition = condition
            });
            return this;
        }

        #region IDbQueryResult

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

        #endregion

        #region IDbAndQueryResult

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

            _parts.Last.Value.Condition = "(" + _parts.Last.Value.Condition;
            return AddCondition(Operation.Or, "");
        }

        public IDbAndQueryResult<TEntity> Or(Expression<Func<TEntity, bool>> orCondition)
        {
            _parts.Last.Value.Condition = "(" + _parts.Last.Value.Condition;

            return AddCondition(Operation.Or, _ExpressionParser.Parse(orCondition) + ")");
        }

        #endregion

        #region OrderedResult

        public IDbOrderedResult<TEntity> OrderBy<TField>(Expression<Func<TEntity, TField>> orderByField)
        {
            return AddCondition(Operation.OrderBy, new FieldGetterExpressionParser<TEntity, TField>().Parse(orderByField));
        }

        public IDbOrderedDescResult<TEntity> Descending()
        {
            return AddCondition(Operation.Desc, "");
        }

        #endregion

        #region IDbFieldFilterResult

        public IDbAndQueryResult<TEntity> In<TField>(IEnumerable<TField> args)
        {

            if (args is IDbResult)
            {
                return InResult((IDbResult)args);
            }
            var _whereCondition = new StringBuilder();

            _whereCondition
                .Append(_FilterField.FieldName)
                .Append(" In (");

            foreach (var arg in args)
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

        #endregion

        private void AppendToConditions(StringBuilder builder)
        {
            _parts.Last.Value.Condition = FinishOrCondition(builder).ToString();
        }

        private StringBuilder FinishOrCondition(StringBuilder builder)
        {
            if (_parts.Last.Value.Operation == Operation.Or)
            {
                builder.Append(")");
            }
            return builder;
        }

        private String AddParameter(Field field, Object value)
        {
            var paramName = String.Format("@{0}{1}{2}", field.FieldName, Environment.TickCount, _parameters.Count);
            _parameters.Add(new KeyValuePair<Key, Object>(new Key(field, paramName), value));
            return paramName;
        }

    }

}
