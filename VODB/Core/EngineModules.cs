using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using VODB.Core.Execution.Executers;
using VODB.Core.Execution.Statements;
using VODB.Core.Infrastructure;
using VODB.Core.Loaders;
using VODB.DbLayer;
using VODB.Sessions;

namespace VODB.Core
{

    enum Commands
    {
        Insert,
        Delete,
        Update,

        Count,
        CountById,

        Select,
        SelectById
    }

    class BindAttribute : NamedAttribute
    {
        public BindAttribute(Commands cmd)
            : base(cmd.ToString())
        { }

    }

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
            Bind<ISession>().To<InternalSession>();
            Bind<IInternalTransaction>().To<Transaction>();
            Bind<IDbConnectionCreator>().To<NameConventionDbConnectionCreator>().InSingletonScope()
                .WithConstructorArgument("providerName", "System.Data.SqlClient");

            Bind<IStatementGetter>().To<InsertGetter>().WhenInjectedInto<InsertExecuter>().InSingletonScope();
            Bind<IStatementGetter>().To<UpdateGetter>().WhenInjectedInto<UpdateExecuter>().InSingletonScope();
            Bind<IStatementGetter>().To<DeleteGetter>().WhenInjectedInto<DeleteExecuter>().InSingletonScope();
            Bind<IStatementGetter>().To<CountGetter>().WhenInjectedInto<CountExecuter>().InSingletonScope();
            Bind<IStatementGetter>().To<CountByIdGetter>().WhenInjectedInto<CountByIdExecuter>().InSingletonScope();

            Bind<IStatementExecuter<int>>().To<InsertExecuter>().InSingletonScope().Named(Commands.Insert.ToString());
            Bind<IStatementExecuter<int>>().To<UpdateExecuter>().InSingletonScope().Named(Commands.Update.ToString());
            Bind<IStatementExecuter<int>>().To<DeleteExecuter>().InSingletonScope().Named(Commands.Delete.ToString());

            Bind<IStatementExecuter<int>>().To<CountExecuter>().InSingletonScope().Named(Commands.Count.ToString());
            Bind<IStatementExecuter<int>>().To<CountByIdExecuter>().InSingletonScope().Named(Commands.CountById.ToString());

            Bind<IStatementExecuter<DbDataReader>>().To<SelectByIdExecuter>().InSingletonScope().Named(Commands.SelectById.ToString());
            Bind<IStatementExecuter<DbDataReader>>().To<SelectExecuter>().InSingletonScope().Named(Commands.Select.ToString());

            Bind<IEntityLoader>().To<FullEntityLoader>().InSingletonScope();
        }
    }

}
