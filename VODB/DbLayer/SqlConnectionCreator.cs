using System;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using VODB.Exceptions;

namespace VODB.DbLayer
{
    internal sealed class SqlConnectionCreator : DbConnectionCreator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DbConnectionCreatorDescendant"/> class.
        /// </summary>
        /// <param name="providerName">Name of the provider.</param>
        /// <param name="connectionStringName"> </param>
        public SqlConnectionCreator()
            : base("System.Data.SqlClient", null)
        {

        }
    }
}
