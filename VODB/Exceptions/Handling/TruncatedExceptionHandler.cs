using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace VODB.Exceptions.Handling
{
    public class TruncatedExceptionHandler : IExceptionHandler
    {

        /// <summary>
        /// Determines whether this instance can handle the specified exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns></returns>
        public Boolean CanHandle(Exception exception)
        {
            return typeof(SqlException).IsAssignableFrom(exception.GetType()) &&
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
    }
}
