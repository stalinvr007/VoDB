using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VODB.Core.Infrastructure;
using VODB.DbLayer;
using VODB.Sessions;

namespace VODB.Core
{

    internal class InfrastructureModule : NinjectModule
    {

        public override void Load()
        {
            Bind(typeof(IFieldMapper<>)).To(typeof(FieldMapper<>)).InSingletonScope();
            Bind(typeof(ITableMapper<>)).To(typeof(TableMapper<>)).InSingletonScope();
            Bind<ITableMapper>().To<TableMapper>();
            Bind<IFieldMapper>().To<FieldMapper>();
            
            Bind<ITSqlCommandHolder>().To<TSqlCommandHolderLazy>();

            Bind<IEntityTables>().To<EntityTables>().InSingletonScope();
        }
    }

    internal class EngineModules : NinjectModule
    {
        public override void Load()
        {
            Bind<IConfiguration>().To<Configuration>().InSingletonScope();
        }
    }

    internal class SessionModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IInternalTransaction>().To<Transaction>();
            Bind<IDbConnectionCreator>().To<NameConventionDbConnectionCreator>();


        }
    }

}
