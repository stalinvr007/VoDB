using System;
using System.Data.SqlClient;

namespace VODB.Exceptions.Handling
{
    class PrimaryKeyExceptionHandler : IExceptionHandler
    {

        public bool CanHandle(Exception exception)
        {
            return exception is SqlException && 
                exception.Message.Contains("Violation of PRIMARY KEY");
        }

        public void Handle(Exception exception)
        {
            throw new PrimaryKeyViolationException(exception);
        }

    }
}
