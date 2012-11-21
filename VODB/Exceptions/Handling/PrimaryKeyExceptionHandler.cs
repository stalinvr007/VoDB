using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace VODB.Exceptions.Handling
{
    class PrimaryKeyExceptionHandler : IExceptionHandler
    {

        public bool CanHandle(Exception exception)
        {
            return typeof(SqlException).IsAssignableFrom(exception.GetType()) && 
                exception.Message.Contains("Violation of PRIMARY KEY");
        }

        public void Handle(Exception exception)
        {
            throw new PrimaryKeyViolationException(exception);
        }

    }
}
