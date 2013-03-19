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

    [TestFixture(typeof(DbNonQueryCommandExecutor), typeof(DbQueryCommandExecutor), typeof(DbScalarCommandExecutor), typeof(DbParameterFactory), typeof(DbOldParameterFactory), typeof(OrderedEntityMapper), typeof(EntityTranslator))]
    public class Executor_Tests<TNonQueryExecutor, TQueryExecutor, TScalarExecutor, TParameterFactory, TOldParameterFactory, TEntityMapper, TTranslator>
        where TNonQueryExecutor : IDbCommandExecutor<int>, new()
        where TQueryExecutor : IDbCommandExecutor<IDataReader>, new()
        where TScalarExecutor : IDbCommandExecutor<Object>, new()
        where TParameterFactory : IDbParameterFactory, new()
        where TOldParameterFactory : IDbParameterFactory, new()
        where TEntityMapper : IEntityMapper, new()
        where TTranslator : IEntityTranslator, new()
    {

        private IEnumerable GetEntities()
        {
            return Utils.TestModels.ToTables().Select(t => t.CreateExistingTestEntity());
        }

        [TestCaseSource("GetEntities")]
        public void SimpleExecutor_Assert_Delete(Object entity)
        {

            Utils.ExecuteWith(con =>
            {
                var trans = con.BeginTransaction();
                try
                {
                    var executor = new SimpleExecutor(
                        new TNonQueryExecutor(),
                        new TQueryExecutor(),
                        new TScalarExecutor(),
                        new CommandFactory(con, trans),
                        new TParameterFactory(),
                        new TOldParameterFactory(),
                        new TEntityMapper(),
                        new TTranslator());

                    try
                    {
                        executor.Delete(entity);
                    }
                    catch (SqlException ex)
                    {
                        if (!ex.Message.Contains("conflicted with the REFERENCE")) 
                            throw ex;
                    }
                    
                }
                finally
                {
                    trans.Rollback();
                }

            });


        }


    }
}
