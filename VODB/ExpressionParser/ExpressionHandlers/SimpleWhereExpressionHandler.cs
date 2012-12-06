using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using VODB.VirtualDataBase;

namespace VODB.ExpressionParser.ExpressionHandlers
{
    class SimpleWhereExpressionHandler : IWhereExpressionHandler
    {
        public bool CanHandle<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : Entity, new()
        {
            return typeof (Entity).IsAssignableFrom(typeof (TEntity)) &&
                   expression.Body is BinaryExpression;
        }

        public KeyValuePair<Field, object> Handle<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : Entity, new()
        {
            var entity = new TEntity();
            
            var body = (BinaryExpression)expression.Body;

            Object value;

            if (body.Right is ConstantExpression)
            {
                var right = (ConstantExpression)body.Right;
                value = right.Value;
            }
            else
            {
                var right = (MemberExpression)body.Right;
                value = Expression.Lambda(right).Compile().DynamicInvoke();    
            }
            
            var field = ((MemberExpression)body.Left).Member.Name;

            Debug.Assert(entity != null, "entity != null");

            var tableField = entity.Table.Fields.First(f => 
                f.PropertyName.Equals(field, StringComparison.InvariantCultureIgnoreCase));

            return new KeyValuePair<Field, object>(tableField, value);
        }
    }
}