using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using VODB.Core;
using VODB.Core.Infrastructure;

namespace VODB.ExpressionParser.ExpressionHandlers
{
    class SimpleWhereExpressionHandler : IWhereExpressionHandler
    {
        public bool CanHandle<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class, new()
        {
            return Engine.IsMapped<TEntity>() &&
                   expression.Body is BinaryExpression;
        }

        public KeyValuePair<Field, object> Handle<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class, new()
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