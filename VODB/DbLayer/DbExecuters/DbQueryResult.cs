using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using VODB.DbLayer.DbCommands;

namespace VODB.DbLayer.DbExecuters
{

    internal sealed class DbQueryResult
    {

        public DbQueryResult(IEnumerable<Object> values)
        {
            Values = values;
        }

        public IEnumerable<Object> Values { get; private set; }

    }


    internal sealed class DbQueryResult<TEntity> : IDbCommandFactory, IDbAndQueryResult<TEntity>, IDbQueryResult<TEntity>
    {

        private readonly IDbCommandFactory _CommandFactory;
        private readonly IQueryExecuter<TEntity> _Executer;
        private readonly StringBuilder _whereCondition;

        public DbQueryResult(IDbCommandFactory commandFactory, IQueryExecuter<TEntity> executer)
        {
            _CommandFactory = commandFactory;
            _Executer = executer;
            _whereCondition = new StringBuilder();
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
            return cmd;
        }

        public IDbAndQueryResult<TEntity> Where(string whereCondition, params object[] args)
        {
            _whereCondition.Append(" Where ").AppendFormat(whereCondition, args);
            return this;
        }

        public IDbAndQueryResult<TEntity> And(string andCondition, params object[] args)
        {
            _whereCondition.Append(" And ").AppendFormat(andCondition, args);
            return this;
        }
    }

}
