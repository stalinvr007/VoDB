using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace VODB.DbLayer.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class ConnectionStringNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionStringNotFound" /> class.
        /// </summary>
        /// <param name="providerName">Name of the provider.</param>
        public ConnectionStringNotFoundException(string providerName)
            : base(String.Format("There's no connection for the provider [{0}].", providerName)) { }
    }
}
