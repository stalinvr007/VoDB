using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using VODB.VirtualDataBase;

namespace VODB.ExpressionParser.ExpressionHandlers
{
    class SimpleWhereExpressionHandler : IWhereExpressionHandler
    {
        public bool CanHandle<TEntity>(Expression<Func<TEntity, bool>> expression)
        {
            return typeof (Entity).IsAssignableFrom(typeof (TEntity)) &&
                   expression.Body is BinaryExpression;
        }

        public KeyValuePair<Field, object> Handle<TEntity>(Expression<Func<TEntity, bool>> expression)
        {
            // TODO: Throw exception if unable to create the instance.
            var entity = Activator.CreateInstance<TEntity>() as Entity;

            var body = (BinaryExpression)expression.Body;

            var right = (MemberExpression)body.Right;
            var value = Expression.Lambda(right).Compile().DynamicInvoke();
            var field = ((MemberExpression)body.Left).Member.Name;

            var tableField = entity.Table.Fields.First(f => f.PropertyName.Equals(field, StringComparison.InvariantCultureIgnoreCase));

            return new KeyValuePair<Field, object>(tableField, value);
        }
    }
}