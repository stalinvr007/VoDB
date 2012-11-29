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
            var pair = expression.GetKeyValue();

            var parameter = pair.Key.Table.TableName + pair.Key.FieldName;

            _ConditionData.Add(new Key(pair.Key, parameter), pair.Value);

            return expression.GetWherePiece(pair.Key.FieldName, parameter);
        }


        public IEnumerable<KeyValuePair<Key, object>> ConditionData
        {
            get { return _ConditionData; }
        }

    }
}
