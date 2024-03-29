using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using VODB.Core;
using VODB.Core.Infrastructure;
using VODB.Exceptions;
using VODB.ExpressionParser;
using VODB.ExpressionParser.ExpressionHandlers;
using VODB.ExpressionParser.TSqlBuilding;

namespace VODB.Extensions
{
    internal static class ExpressionsExtensions
    {
        private static readonly IConfiguration Configuration = Engine.Get<IConfiguration>();

        /// <summary>
        /// Gets the key value.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        /// <exception cref="WhereExpressionHandlerNotFoundException"></exception>
        public static KeyValuePair<Field, object> GetKeyValue<TEntity>(this Expression<Func<TEntity, bool>> expression)
            where TEntity : class, new()
        {
            foreach (IWhereExpressionHandler handler in Configuration.WhereExpressionHandlers
                .Where(handler => handler.CanHandle(expression)))
            {
                return handler.Handle(expression);
            }

            throw new WhereExpressionHandlerNotFoundException();
        }


        /// <summary>
        /// Builds the SQL.
        /// </summary>
        /// <param name="parser">The parser.</param>
        /// <returns></returns>
        public static ITSqlBuilder BuildSql(this IExpressionBodyParser parser)
        {
            ITSqlBuilder builder = Configuration.TSqlBuilders.Where(b => b.CanBuild(parser)).FirstOrDefault();

            if (builder == null)
            {
                throw new TSqlBuilderNotFoundException();
            }

            return builder;
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
        public static String GetWherePiece<TModel>(this Expression<Func<TModel, bool>> expression, String fieldName,
                                                   String parameterName)
        {
            foreach (IWhereExpressionFormatter formatter in Configuration.WhereExpressionFormatters
                .Where(f => f.CanFormat(expression.Body.NodeType)))
            {
                return formatter.Format(fieldName, parameterName);
            }

            throw new WhereExpressionFormatterNotFoundException();
        }
    }
}