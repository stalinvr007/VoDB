using System;

namespace VODB.Exceptions.Handling
{
    public interface IExceptionHandler
    {
        /// <summary>
        /// Determines whether this instance can handle the specified exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns></returns>
        Boolean CanHandle(Exception exception);

        /// <summary>
        /// Handles the specified exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        void Handle(Exception exception);

    }
}
