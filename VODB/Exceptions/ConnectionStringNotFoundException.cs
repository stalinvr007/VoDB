using System;

namespace VODB.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class ConnectionStringNotFoundException : Exception
    {
        public ConnectionStringNotFoundException(string providerName, string connectionStringName)
            : base(String.Format("There's no connection for the provider [{0}] and ConnectionString Name [{1}].", providerName, connectionStringName)) { }
    }
}
