using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using VODB.Exceptions;
using VODB.VirtualDataBase;

namespace VODB.Extensions
{
    static class ExpressionsExtensions
    {

        /// <summary>
        /// Gets the key value.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        /// <exception cref="WhereExpressionHandlerNotFoundException"></exception>
        public static KeyValuePair<Field, object> GetKeyValue<TEntity>(this Expression<Func<TEntity, bool>> expression)
             where TEntity : Entity, new()
        {
            foreach (var handler in Configuration.WhereExpressionHandlers
                .Where(handler => handler.CanHandle(expression)))
            {
                return handler.Handle(expression);
            }

            throw new WhereExpressionHandlerNotFoundException();
        }


        /// <summary>
        /// Gets the where piece.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <returns></returns>
        /// <exception cref="VODB.Exceptions.WhereExpressionFormatterNotFoundException"></exception>
        public static String GetWherePiece<TModel>(this Expression<Func<TModel, bool>> expression, String fieldName, String parameterName)
        {
            foreach (var formatter in Configuration.WhereExpressionFormatters
                .Where(f => f.CanFormat(expression.Body.NodeType)))
            {
                return formatter.Format(fieldName, parameterName);
            }

            throw new WhereExpressionFormatterNotFoundException();
        }

    }
}
