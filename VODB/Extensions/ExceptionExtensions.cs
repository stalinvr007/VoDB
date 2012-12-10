using System;

namespace VODB.Extensions
{
    internal static class ExceptionExtensions
    {

        /// <summary>
        /// Handles the exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public static void HandleException(this Exception exception)
        {
            foreach (var handler in Configuration.ExceptionHandlers)
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
