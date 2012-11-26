using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using VODB.Extensions;
using VODB.VirtualDataBase;
using System.Linq;

namespace VODB.ExpressionParser
{
    class ComparatorExpressionParser<TEntity> : IWhereExpressionParser<TEntity> 
        where TEntity : Entity, new()
    {

        private readonly IDictionary<string, object> _ConditionData = new Dictionary<string, object>(); 

        public String Parse(Expression<Func<TEntity, Boolean>> expression)
        {
            var pair = expression.GetKeyValue();

            var parameter = pair.Key.Table.TableName + pair.Key.FieldName;

            _ConditionData.Add(parameter, pair.Value);

            return String.Format("{0} = @{1}", pair.Key.FieldName, parameter);
        }


        public IEnumerable<KeyValuePair<string, object>> ConditionData
        {
            get { return _ConditionData; }
        }

    }
}
