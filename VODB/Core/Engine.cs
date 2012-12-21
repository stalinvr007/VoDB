using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VODB.Core.Infrastructure;

namespace VODB.Core
{
    internal static class Engine
    {

        static IKernel kernel = new StandardKernel(
            new InfrastructureModule(), 
            new SessionModule(),
            new EngineModules(),
            new ConfigurationModule()
        );

        static IEntityTables _tables = Get<IEntityTables>();
        
        #region Kernel Wrapper

        public static TClass Get<TClass>()
        {
            return kernel.Get<TClass>();
        }

        public static TClass Get<TClass>(Commands cmd)
        {
            return kernel.Get<TClass>(cmd.ToString());
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
