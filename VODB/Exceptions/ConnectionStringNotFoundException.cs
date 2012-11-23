using System;

namespace VODB.Exceptions
{

    /// <summary>
    /// 
    /// </summary>
    public class ConnectionStringNotFoundException : VodbException
    {
        public ConnectionStringNotFoundException(string providerName, string connectionStringName)
            : base("There's no connection for the provider [{0}] and ConnectionString Name [{1}].", providerName, connectionStringName) { }
    }
}
