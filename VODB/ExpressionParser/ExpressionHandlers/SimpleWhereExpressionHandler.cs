using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using VODB.Infrastructure;

namespace VODB.ExpressionParser.ExpressionHandlers
{
    class SimpleWhereExpressionHandler : IWhereExpressionHandler
    {
        public bool CanHandle<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : Entity, new()
        {
            return typeof(Entity).IsAssignableFrom(typeof(TEntity)) &&
                   expression.Body is BinaryExpression;
        }

        public KeyValuePair<Field, object> Handle<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : Entity, new()
        {            
            var parser = new ExpressionBodyParser(expression.Body)
            {
                Entity = new TEntity()
            };
            parser.Parse();
            
            return new KeyValuePair<Field, object>(parser.Field, parser.Value);
        }
    }
}