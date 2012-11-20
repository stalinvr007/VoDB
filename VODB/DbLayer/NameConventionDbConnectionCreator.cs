using System;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using VODB.Exceptions;

namespace VODB.DbLayer
{
    public class NameConventionDbConnectionCreator : DbConnectionCreator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NameConventionDbConnectionCreator"/> class.
        /// </summary>
        /// <param name="providerName">Name of the provider.</param>
        /// <param name="connectionStringName"> </param>
        public NameConventionDbConnectionCreator(String providerName)
            : base(providerName, Environment.MachineName)
        {

        }
        
    }
}
