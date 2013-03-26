using ImpromptuInterface;
using ImpromptuInterface.Dynamic;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VODB.DbLayer;
using VODB.EntityMapping;
using VODB.EntityTranslation;
using VODB.Executors;
using VODB.Infrastructure;
using VODB.Tests.Models.Northwind;

namespace VODB.Tests.Executors
{

    class CommandFactory : IDbCommandFactory
    {
        private readonly DbConnection _Connection;
        private readonly DbTransaction _Transaction;
        public CommandFactory(DbConnection connection, DbTransaction transaction)
        {
            _Transaction = transaction;
            _Connection = connection;
        }
        public IVodbCommand MakeCommand()
        {
            IVodbCommand cmd = new VodbCommand(_Connection.CreateCommand());
            cmd.SetTransaction(_Transaction);
            return cmd;
        }
    }

    [TestFixture]
    public class DeleteExecutor_Tests
    {

        private static IEnumerable GetEntities()
        {
            return Utils.TestModels.ToTables()
                .Select(t => t.CreateExistingTestEntity())
                .Select(t => new TestCaseData(t).SetName("DeleteExecutor_Assert<" +t.GetType().Name + ">"));
        }

        [TestCaseSource("GetEntities")]
        public void DeleteExecutor_Assert<TEntity>(TEntity entity) where TEntity : new()
        {
            Utils.ExecuteWith((con, trans) =>
            {
                var deleteExecutor = new DeleteExecutor(
                    new DbNonQueryCommandExecutor(),
                    new EntityTranslator(),
                    new CommandFactory(con, trans),
                    new DbParameterFactory(),
                    new DbOldParameterFactory()
                );

                try
                {
                    deleteExecutor.Delete(entity);
                }
                catch (SqlException ex)
                {
                    if (!ex.Message.Contains("conflicted with the REFERENCE"))
                        throw ex;
                }
            });
            
        }

    }

    [TestFixture]
    public class UpdateExecutor_Tests
    {

        private static IEnumerable GetEntities()
        {
            return Utils.TestModels.ToTables()
                .Select(t => t.CreateExistingTestEntity())
                .Select(t => new TestCaseData(t).SetName("UpdateExecutor_Assert<" + t.GetType().Name + ">"));
        }

        [TestCaseSource("GetEntities")]
        public void UpdateExecutor_Assert<TEntity>(TEntity entity) where TEntity : new()
        {
            Utils.ExecuteWith((con, trans) =>
            {
                var UpdateExecutor = new UpdateExecutor(
                    new DbNonQueryCommandExecutor(),
                    new EntityTranslator(),
                    new CommandFactory(con, trans),
                    new DbParameterFactory(),
                    new DbOldParameterFactory()
                );

                try
                {
                    UpdateExecutor.Update(entity);
                }
                catch (SqlException ex)
                {
                    if (!ex.Message.Contains("conflicted with the REFERENCE"))
                        throw ex;
                }
            });

        }

    }

    [TestFixture]
    public class SelectByIdExecutor_Tests
    {

        private static IEnumerable GetEntities()
        {
            return Utils.TestModels.ToTables()
                .Where(t => t.Name != "CustomerCustomerDemo")
                .Where(t => t.Name != "CustomerDemographics")
                .Select(t => t.CreateExistingTestEntity())
                .Select(t => new TestCaseData(t).SetName("SelectByIdExecutor_Assert<" + t.GetType().Name + ">"));
        }

        [TestCaseSource("GetEntities")]
        public void SelectByIdExecutor_Assert<TEntity>(TEntity entity) where TEntity : new()
        {
            Utils.ExecuteWith((con, trans) =>
            {
                var LoaderExecutor = new LoadByIdExecutor(
                    new DbQueryCommandExecutor(),
                    new OrderedEntityMapper(),
                    new EntityTranslator(),
                    new CommandFactory(con, trans),
                    new DbParameterFactory(),
                    new DbOldParameterFactory()
                );

                Assert.That(LoaderExecutor.Load(entity), Is.Not.Null);
                
            });

        }

    }

    [TestFixture]
    public class InsertExecutor_Tests
    {

        private static IEnumerable GetEntities()
        {
            return Utils.TestModels.ToTables()
                .Where(t => t.Name != "CustomerCustomerDemo")
                .Where(t => t.Name != "CustomerDemographics")
                .Select(t => t.CreateUnExistingTestEntity())
                .Select(t => new TestCaseData(t).SetName("InsertExecutor_Assert<" + t.GetType().Name + ">"));
        }

        [TestCaseSource("GetEntities")]
        public void InsertExecutor_Assert<TEntity>(TEntity entity) where TEntity : new()
        {
            Utils.ExecuteWith((con, trans) =>
            {
                var LoaderExecutor = new InsertExecutor(
                    new DbScalarCommandExecutor(),
                    new EntityTranslator(),
                    new CommandFactory(con, trans),
                    new DbParameterFactory(),
                    new DbOldParameterFactory()
                );

                Assert.That(LoaderExecutor.Insert(entity), Is.Not.Null);

            });

        }

    }
}
