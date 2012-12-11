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
            var parser = new ExpressionBodyParser(expression);
            
            parser.Parse();

            var sqlBuilder = parser.BuildSql();

            foreach (var param in sqlBuilder.Parameters)
            {
                _ConditionData.Add(param);
            }
            
            return sqlBuilder.Build();
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
