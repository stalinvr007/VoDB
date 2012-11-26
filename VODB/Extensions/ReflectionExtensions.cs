using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using VODB.Exceptions;
using VODB.VirtualDataBase;

namespace VODB.Extensions
{
    static class ReflectionExtensions
    {

        public static TAttribute GetAttribute<TAttribute>(this PropertyInfo info)
            where TAttribute : class
        {
            return info.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() as TAttribute;
        }

        /// <summary>
        /// Gets the key value.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        /// <exception cref="VODB.Exceptions.WhereExpressionHandlerNotFoundException"></exception>
        public static KeyValuePair<Field, object> GetKeyValue<TModel>(this Expression<Func<TModel, bool>> expression)
        {
            foreach (var handler in Configuration.WhereExpressionHandlers
                .Where(handler => handler.CanHandle(expression)))
            {
                return handler.Handle(expression);
            }

            throw new WhereExpressionHandlerNotFoundException();
        }
    }
}
