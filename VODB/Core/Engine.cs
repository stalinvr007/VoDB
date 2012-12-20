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
            new EngineModule()
        );

        static IEntityTables _tables = Get<IEntityTables>();

        public static TClass Get<TClass>()
        {
            return kernel.Get<TClass>();
        }

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
        
    }
}
