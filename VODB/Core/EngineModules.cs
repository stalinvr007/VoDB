using Castle.DynamicProxy;
using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using VODB.Core.Execution.DbParameterSetters;
using VODB.Core.Execution.Executers;
using VODB.Core.Execution.Executers.DbResults;
using VODB.Core.Execution.Statements;
using VODB.Core.Infrastructure;
using VODB.Core.Loaders;
using VODB.Core.Loaders.Factories;
using VODB.Core.Loaders.FieldSetters;
using VODB.DbLayer;
using VODB.EntityValidators;
using VODB.EntityValidators.Fields;
using VODB.Exceptions.Handling;
using VODB.ExpressionParser;
using VODB.ExpressionParser.ExpressionHandlers;
using VODB.ExpressionParser.TSqlBuilding;
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
            Bind<IStatementGetter>().To<SelectByIdGetter>().WhenInjectedInto<SelectByIdExecuter>().InSingletonScope();

            Bind<IStatementExecuter<int>>().To<InsertExecuter>().InSingletonScope().Named(Commands.Insert.ToString());
            Bind<IStatementExecuter<int>>().To<UpdateExecuter>().InSingletonScope().Named(Commands.Update.ToString());
            Bind<IStatementExecuter<int>>().To<DeleteExecuter>().InSingletonScope().Named(Commands.Delete.ToString());

            Bind<IStatementExecuter<int>>().To<CountExecuter>().InSingletonScope().Named(Commands.Count.ToString());
            Bind<IStatementExecuter<int>>().To<CountByIdExecuter>().InSingletonScope().Named(Commands.CountById.ToString());

            Bind<IStatementExecuter<DbDataReader>>().To<SelectByIdExecuter>().InSingletonScope().Named(Commands.SelectById.ToString());

            Bind<IQueryResultGetter>().To<QueryResultGetter>().InSingletonScope();
            Bind<IEntityLoader>().To<FullEntityLoader>().InSingletonScope();
            Bind<IEntityFactory>().To<EntityProxyFactory>().InSingletonScope();;
        }
    }

    internal class ConfigurationModule : NinjectModule
    {
        public override void Load()
        {
            /* Field Validators */
            Bind<IFieldValidator>().To<StringFieldIsFilled>();
            Bind<IFieldValidator>().To<NumberFieldIsFilled>();
            Bind<IFieldValidator>().To<DateTimeFieldIsFilled>();
            Bind<IFieldValidator>().To<DbEntityFieldIsFilled>();
            Bind<IFieldValidator>().To<RefTypeFieldIsFilled>();

            /* Entity Validators */
            Bind<IEntityValidator>().To<RequiredFieldsValidator>();
            Bind<IEntityValidator>().To<KeyFilledValidator>();

            /* Field Setters */
            Bind<IFieldSetter>().To<BasicFieldSetter>();
            Bind<IFieldSetter>().To<DbEntityFieldSetter>();

            /* Parameter Setters */
            Bind<IParameterSetter>().To<BasicParameterSetter>();
            Bind<IParameterSetter>().To<DbEntityParameterSetter>();
            Bind<IParameterSetter>().To<DateTimeParameterSetter>();
            Bind<IParameterSetter>().To<DecimalParameterSetter>();
            Bind<IParameterSetter>().To<ByteArrayParameterSetter>();
            Bind<IParameterSetter>().To<GuidParameterSetter>();

            /* Exception Handlers */
            Bind<IExceptionHandler>().To<PrimaryKeyExceptionHandler>();
            Bind<IExceptionHandler>().To<UniqueKeyExceptionHandler>();
            Bind<IExceptionHandler>().To<TruncatedExceptionHandler>();

            /* Where Expression Handlers */
            Bind<IWhereExpressionHandler>().To<SimpleWhereExpressionHandler>();

            /* Where Expression Formatters */
            Bind<IWhereExpressionFormatter>().To<EqualityWhereExpressionFormatter>();
            Bind<IWhereExpressionFormatter>().To<NonEqualityWhereExpressionFormatter>();
            Bind<IWhereExpressionFormatter>().To<GreaterOrEqualWhereExpressionFormatter>();
            Bind<IWhereExpressionFormatter>().To<SmallerOrEqualWhereExpressionFormatter>();
            Bind<IWhereExpressionFormatter>().To<SmallerWhereExpressionFormatter>();
            Bind<IWhereExpressionFormatter>().To<GreaterWhereExpressionFormatter>();

            /* Sql builders for expressions */
            Bind<ITSqlBuilder>().To<SimpleWhereTSqlBuilder>();
            Bind<ITSqlBuilder>().To<ComplexTSqlBuilder>();
        }
    }

}
