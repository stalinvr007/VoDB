using System;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using VODB.Exceptions;

namespace VODB.DbLayer
{
    public class DbConnectionCreator : IDbConnectionCreator
    {
        private readonly String _ProviderName;
        private readonly String _ConnectionStringName;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbConnectionCreator" /> class.
        /// </summary>
        /// <param name="providerName">Name of the provider.</param>
        /// <param name="connectionStringName"> </param>
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
                    if (connection != null)
                    {
                        connection.ConnectionString = connectionString;
                        return connection;
                    }
                }
                catch (Exception ex)
                {
                    throw new UnableToCreateDbConnectionException(_ProviderName, ex);
                }
            }

            throw new ConnectionStringNotFoundException(_ProviderName, _ConnectionStringName);
        }

        protected static string GetConnectionStringByProvider(string providerName, String connectionStringName = null)
        {
            ConnectionStringSettingsCollection settings = ConfigurationManager.ConnectionStrings;

            if (settings != null)
            {
                return settings.Cast<ConnectionStringSettings>()
                    .Where(cs => cs.ProviderName == providerName)
                    .Where(cs => connectionStringName == null || cs.Name == connectionStringName)
                    .Select(cs => cs.ConnectionString)
                    .FirstOrDefault();
            }
            return null;
        }


    }
}
