using System;
using Ninject;
using Ninject.Parameters;
using VODB.Core.Infrastructure;

namespace VODB.Core
{
    internal static class Engine
    {
        private static readonly IKernel kernel = new StandardKernel(
            new InfrastructureModule(),
            new SessionModule(),
            new EngineModules(),
            new ConfigurationModule()
            );

        private static readonly IEntityTables _tables = Get<IEntityTables>();

        public static IConfiguration Configuration = Get<IConfiguration>();

        #region Kernel Wrapper

        public static TClass Get<TClass>()
        {
            return kernel.Get<TClass>();
        }

        public static TClass Get<TClass>(Commands cmd)
        {
            return kernel.Get<TClass>(cmd.ToString());
        }

        public static TClass Get<TClass>(String argName, Object value)
        {
            return kernel.Get<TClass>(new ConstructorArgument(argName, value));
        }

        #endregion

        #region IEntityTablesWrapper

        public static Table GetTable<TEntity>()
        {
            return _tables.GetTable<TEntity>();
        }

        public static Table GetTable(Type type)
        {
            return _tables.GetTable(type);
        }

        public static void Map<TEntity>()
        {
            _tables.Map<TEntity>();
        }

        public static void Map(Type type)
        {
            _tables.Map(type);
        }

        public static Boolean IsMapped(Type entityType)
        {
            return _tables.IsMapped(entityType);
        }

        public static Boolean IsMapped<TEntity>()
        {
            return _tables.IsMapped<TEntity>();
        }

        #endregion
    }
}