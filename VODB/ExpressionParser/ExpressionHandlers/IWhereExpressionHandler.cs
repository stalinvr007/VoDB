using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using VODB.VirtualDataBase;

namespace VODB.ExpressionParser.ExpressionHandlers
{
    public interface IWhereExpressionHandler
    {

        /// <summary>
        /// Determines whether this instance can handle the specified expression.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns>
        ///   <c>true</c> if this instance can handle the specified expression; otherwise, <c>false</c>.
        /// </returns>
        bool CanHandle<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : Entity, new();

        /// <summary>
        /// Handles the specified expression.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns>The fieldName and the value.</returns>
        KeyValuePair<Field, object> Handle<TEntity>(Expression<Func<TEntity, Boolean>> expression) where TEntity : Entity, new();

    }

}