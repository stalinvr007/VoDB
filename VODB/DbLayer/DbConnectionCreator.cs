using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Text;
using VODB.DbLayer.Exceptions;

namespace VODB.DbLayer
{
    internal sealed class DbConnectionCreator : IDbConnectionCreator
    {
        private readonly String _ProviderName;
        private readonly String _ConnectionStringName;
        /// <summary>
        /// Initializes a new instance of the <see cref="DbConnectionCreator" /> class.
        /// </summary>
        /// <param name="providerName">Name of the provider.</param>
        public DbConnectionCreator(String providerName, String connectionStringName = null)
        {
            _ConnectionStringName = connectionStringName;
            _ProviderName = providerName;
        }

        public DbConnection Create()
        {
            String connectionString = GetConnectionStringByProvider(_ProviderName, _ConnectionStringName);

            if (connectionString != null)
            {
                try
                {
                    DbProviderFactory factory = DbProviderFactories.GetFactory(_ProviderName);
                    var connection = factory.CreateConnection();
                    connection.ConnectionString = connectionString;
                    return connection;
                }
                catch (Exception ex)
                {
                    throw new UnableToCreateDbConnectionException(_ProviderName, ex);
                }
            }

            throw new ConnectionStringNotFoundException(_ProviderName);
        }

        static string GetConnectionStringByProvider(string providerName, String connectionStringName = null)
        {
            ConnectionStringSettingsCollection settings = ConfigurationManager.ConnectionStrings;

            if (settings != null)
            {
                foreach (ConnectionStringSettings cs in settings)
                {
                    if (cs.ProviderName == providerName )
                    {
                        if (connectionStringName == null || connectionStringName != null && cs.Name == connectionStringName)
                        {
                            return cs.ConnectionString;
                        }
                    }
                }
            }
            return null;
        }


    }
}
