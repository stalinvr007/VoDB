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
        public System.Data.Common.DbCommand MakeCommand()
        {
            DbCommand cmd = _Connection.CreateCommand();
            cmd.Transaction = _Transaction;
            return cmd;
        }
    }

    [TestFixture]
    public class DeleteExecutor_Tests
    {

        private static IEnumerable GetEntities()
        {
            return Utils.TestModels.ToTables()
                .Select(t => t.CreateExistingTestEntity());
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
}
