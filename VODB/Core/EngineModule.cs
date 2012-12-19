using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VODB.Core.Infrastructure;

namespace VODB.Core
{

    internal class InfrastructureModule : NinjectModule
    {

        public override void Load()
        {
            Bind(typeof(IFieldMapper<>)).To(typeof(FieldMapper<>)).InSingletonScope();
            Bind(typeof(ITableMapper<>)).To(typeof(TableMapper<>)).InSingletonScope();
            Bind<ITSqlCommandHolder>().To<TSqlCommandHolderLazy>();

            Bind<IEntityTables>().To<EntityTables>().InSingletonScope();
        }
    }

    internal class EngineModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IConfiguration>().To<Configuration>().InSingletonScope();
        }
    }
}
