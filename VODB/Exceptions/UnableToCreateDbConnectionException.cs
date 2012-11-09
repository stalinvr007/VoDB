using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Text;
using VODB.DbLayer.Exceptions;

namespace VODB.DbLayer.Exceptions
{
    public class UnableToCreateDbConnectionException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnableToCreateDbConnectionException" /> class.
        /// </summary>
        /// <param name="providerName">Name of the provider.</param>
        public UnableToCreateDbConnectionException(string providerName, Exception innerException)
            : base(String.Format("Error creating the DbConnection object for the [{0}] provider. See inner exception for details.", providerName), innerException)
        {

        }
    }
}
