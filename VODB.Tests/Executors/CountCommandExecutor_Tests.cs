using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VODB.DbLayer;
using VODB.Executors;
using VODB.Infrastructure;
using VODB.Tests.Models.Northwind;

namespace VODB.Tests.Executors
{
    [TestFixture(typeof(DbScalarCommandExecutor))]
    public class CountCommandExecutor_Tests<TCommandExecutor> where TCommandExecutor : IDbCommandExecutor<Object>, new()
    {
        static IDbCommandExecutor<Object> executor = new TCommandExecutor();

        private static IEnumerable GetTables()
        {
            return Utils.TestModels.ToTables()
                .Select(t => new TestCaseData(t).Returns(Utils.RecordCounts[t.EntityType]));
        }

        [TestCaseSource("GetTables")]
        public int IDbCommandExecutor_CountCommand_Assert(ITable table)
        {
            int count = 0;

            Utils.ExecuteWith(connection =>
            {
                var cmd = new VodbCommand(connection.CreateCommand());

                cmd.SetCommandText(table.SqlCount);

                count = (int)executor.ExecuteCommand(cmd);

            });

            return count;
        }

    }
}
