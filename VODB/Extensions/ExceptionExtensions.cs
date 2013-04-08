using System;
using VODB.Core;
using VODB.Exceptions.Handling;

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
            if (Config.Handlers.CanHandle(exception))
            {
                Config.Handlers.Handle(exception);
                return;
            }

            throw exception;
        }

        public static TResult CaptureExceptions<TResult>(this ISession session, Func<ISession, TResult> func)
        {
            try
            {
                return func(session);
            }
            catch (Exception ex)
            {
                ex.HandleException();
                throw ex;
            }
        }

        public static void CaptureExceptions(this ISession session, Action<ISession> action)
        {
            try
            {
                action(session);
            }
            catch (Exception ex)
            {
                ex.HandleException();
                throw ex;
            }
        }

    }
}