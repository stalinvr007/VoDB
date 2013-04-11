using System;
using VODB.Core;
using VODB.Exceptions;
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
        }

        public static TResult CaptureExceptions<TResult>(this IInternalSession session, Func<IInternalSession, TResult> func)
        {
            try
            {
                return func(session);
            }
            catch (Exception ex)
            {
                ex.HandleException();

                // If no one handles the exception Wrap it!
                throw new VodbException(ex, "");
            }
        }

        public static void CaptureExceptions(this IInternalSession session, Action<IInternalSession> action)
        {
            session.CaptureExceptions<Object>(s =>
            {
                action(s);
                return null;
            });
        }

    }
}