using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using VODB.DbLayer.DbCommands;
using VODB.ExpressionParser;
using VODB.Extensions;

namespace VODB.DbLayer.DbExecuters
{

    internal sealed class DbQueryResult<TEntity> : IDbCommandFactory, IDbAndQueryResult<TEntity>, IDbQueryResult<TEntity>
        where TEntity : Entity, new()
    {

        private readonly IDbCommandFactory _CommandFactory;
        private readonly IQueryExecuter<TEntity> _Executer;
        private readonly StringBuilder _whereCondition;
        private readonly IWhereExpressionParser<TEntity> _ExpressionParser;


        public DbQueryResult(IDbCommandFactory commandFactory, IQueryExecuter<TEntity> executer)
        {
            _CommandFactory = commandFactory;
            _Executer = executer;
            _whereCondition = new StringBuilder();
            _ExpressionParser = new ComparatorExpressionParser<TEntity>();
        }

        public IEnumerator<TEntity> GetEnumerator()
        {
            return _Executer.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public DbCommand Make()
        {
            var cmd = _CommandFactory.Make();
            cmd.CommandText += _whereCondition.ToString();

            foreach (var data in _ExpressionParser.ConditionData)
            {
                cmd.SetParameter(data.Key.Field, data.Key.ParamName, data.Value);
            }

            return cmd;
        }

        public IDbAndQueryResult<TEntity> Where(string whereCondition, params object[] args)
        {
            // TODO: change de argument set to use the DbParameter.
            _whereCondition.Append(" Where ").AppendFormat(whereCondition, args);
            return this;
        }

        public IDbAndQueryResult<TEntity> And(string andCondition, params object[] args)
        {
            // TODO: change de argument set to use the DbParameter.
            _whereCondition.Append(" And ").AppendFormat(andCondition, args);
            return this;
        }


        public IDbAndQueryResult<TEntity> Where(System.Linq.Expressions.Expression<Func<TEntity, bool>> whereCondition)
        {
            return Where(_ExpressionParser.Parse(whereCondition));
        }


        public IDbAndQueryResult<TEntity> And(System.Linq.Expressions.Expression<Func<TEntity, bool>> andCondition)
        {
            return And(_ExpressionParser.Parse(andCondition));
        }
    }

}
