using System;

namespace VODB.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class ConnectionStringNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionStringNotFoundException" /> class.
        /// </summary>
        /// <param name="providerName">Name of the provider.</param>
        public ConnectionStringNotFoundException(string providerName)
            : base(String.Format("There's no connection for the provider [{0}].", providerName)) { }
    }
}
