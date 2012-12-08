using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq.Expressions;
using System.Linq;
using System.Text;
using VODB.DbLayer.DbCommands;
using VODB.DbLayer.DbExecuters;
using VODB.ExpressionParser;
using VODB.Extensions;
using VODB.VirtualDataBase;

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
            _whereCondition
                .Append(_FilterField.FieldName)
                .Append(" in (");

            foreach (var arg in args)
            {
                var paramName = String.Format("@{0}in{1}", _FilterField.FieldName, _parameters.Count);

                _whereCondition
                    .Append(paramName)
                    .Append(", ");

                _parameters.Add(new KeyValuePair<Key, Object>(new Key(_FilterField, paramName),arg));
            }

            _whereCondition
                .Remove(_whereCondition.Length - 2, 2)
                .Append(") ");

            return this;
        }

        public IDbAndQueryResult<TEntity> Between<TField>(TField firstValue, TField secondValue)
        {
            var paramName1 = String.Format("@{0}in{1}", _FilterField.FieldName, _parameters.Count);
            _parameters.Add(new KeyValuePair<Key, Object>(new Key(_FilterField, paramName1), firstValue));

            var paramName2 = String.Format("@{0}in{1}", _FilterField.FieldName, _parameters.Count);
            _parameters.Add(new KeyValuePair<Key, Object>(new Key(_FilterField, paramName2), secondValue));

            _whereCondition
                .Append(_FilterField.FieldName)
                .AppendFormat(" Between {0} And {1}", paramName1, paramName2);

            return this;
        }

        #endregion



    }

}
