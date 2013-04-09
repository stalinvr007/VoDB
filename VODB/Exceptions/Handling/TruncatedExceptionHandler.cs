using System;
using System.Data.SqlClient;

namespace VODB.Exceptions.Handling
{
    public class OnTruncatedExceptionHandler : IExceptionHandler
    {
        #region IExceptionHandler Members

        /// <summary>
        /// Determines whether this instance can handle the specified exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns></returns>
        public Boolean CanHandle(Exception exception)
        {
            return exception is SqlException &&
                   exception.Message.Contains("String or binary data would be truncated.");
        }

        /// <summary>
        /// Handles the specified exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public void Handle(Exception exception)
        {
            throw new TruncatedException(exception);
        }

        #endregion
    }
}