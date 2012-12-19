using System;
using System.Collections.Generic;

namespace VODB.ExpressionParser
{
    interface IWhereExpressionParser<TEntity> : IExpressionParser<Func<TEntity, bool>>
        where TEntity : new()
    {

        /// <summary>
        /// Gets the condition data.
        /// </summary>
        /// <value>
        /// The condition data.
        /// </value>
        IEnumerable<KeyValuePair<Key, object>>  ConditionData { get; }


        /// <summary>
        /// Clears the data.
        /// </summary>
        void ClearData();
    }
}