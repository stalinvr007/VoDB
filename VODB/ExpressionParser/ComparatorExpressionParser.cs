using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using VODB.Extensions;

namespace VODB.ExpressionParser
{

    class ComparatorExpressionParser<TEntity> : IWhereExpressionParser<TEntity> 
        where TEntity : Entity, new()
    {
        private readonly IDictionary<Key, object> _ConditionData = new Dictionary<Key, object>();

        public String Parse(Expression<Func<TEntity, Boolean>> expression)
        {
            var parser = new ExpressionBodyParser(expression.Body)
            {
                Entity = new TEntity()
            };

            parser.Parse();

            var sqlBuilder = parser.BuildSql();
            string result = sqlBuilder.Build(_ConditionData.Count);

            foreach (var param in sqlBuilder.Parameters)
            {
                _ConditionData.Add(param);
            }

            sqlBuilder.ClearParameters();

            return result;
        }

        public IEnumerable<KeyValuePair<Key, object>> ConditionData
        {
            get { return _ConditionData; }
        }
        
        public void ClearData()
        {
            _ConditionData.Clear();
        }
    }
}
