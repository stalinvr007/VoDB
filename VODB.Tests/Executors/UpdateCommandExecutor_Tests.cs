using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VODB.Executors;
using VODB.Infrastructure;
using VODB.Tests.Models.Northwind;

namespace VODB.Tests.Executors
{
    [TestFixture(typeof(DbNonQueryCommandExecutor))]
    public class UpdateCommandExecutor_Tests<TCommandExecutor> where TCommandExecutor : IDbCommandExecutor<int>, new()
    {
        static IDbCommandExecutor<int> executor = new TCommandExecutor();

        private static IEnumerable GetTables()
        {
            return Utils.TestModels
                .ToTables()
                .Where(t => t.Name != "CustomerCustomerDemo")
                .Where(t => t.Name != "CustomerDemographics");
        }

        [TestCaseSource("GetTables")]
        public void IDbCommandExecutor_UpdateCommand_Assert(ITable table)
        {
            var entity = table.CreateExistingTestEntity();

            Utils.ExecuteWith(connection =>
            {
                var trans = connection.BeginTransaction();
                try
                {
                    var cmd = connection.CreateCommand();
                    cmd.Transaction = trans;

                    cmd.CommandText = table.SqlUpdate;

                    foreach (var field in table.Fields)
                    {
                        var param = cmd.CreateParameter();
                        param.ParameterName = "@" + field.Name;
                        param.Value = field.GetFieldFinalValue(entity);

                        if (param.Value == null)
                        {
                            param.Value = DBNull.Value;
                        }

                        cmd.Parameters.Add(param);
                    }

                    foreach (var field in table.Keys)
                    {
                        var param = cmd.CreateParameter();
                        param.ParameterName = "@old" + field.Name;
                        param.Value = field.GetFieldFinalValue(entity);

                        if (param.Value == null)
                        {
                            Assert.Fail("The key {0} was not provided.", param.ParameterName);
                        }

                        cmd.Parameters.Add(param);
                    }

                    Assert.That(cmd.ExecuteNonQuery(), Is.EqualTo(1));
                }
                finally
                {
                    trans.Rollback();
                }
            });

        }

    }
}
