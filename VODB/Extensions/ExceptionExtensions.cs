using System;
using VODB.Core;
using VODB.Exceptions.Handling;

namespace VODB.Extensions
{
    internal static class ExceptionExtensions
    {
        private static readonly IConfiguration Configuration = Engine.Get<IConfiguration>();

        /// <summary>
        /// Handles the exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public static void HandleException(this Exception exception)
        {
            foreach (IExceptionHandler handler in Configuration.ExceptionHandlers)
            {
                if (handler.CanHandle(exception))
                {
                    handler.Handle(exception);
                    return;
                }
            }

            throw exception;
        }
    }
}