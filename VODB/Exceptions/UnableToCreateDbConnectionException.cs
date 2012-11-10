using System;

namespace VODB.Exceptions
{
    public class UnableToCreateDbConnectionException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnableToCreateDbConnectionException" /> class.
        /// </summary>
        /// <param name="providerName">Name of the provider.</param>
        /// <param name="innerException"> </param>
        public UnableToCreateDbConnectionException(string providerName, Exception innerException)
            : base(String.Format("Error creating the DbConnection object for the [{0}] provider. See inner exception for details.", providerName), innerException)
        {

        }
    }
}
