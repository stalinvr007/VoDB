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
    [TestFixture(typeof(DbQueryCommandExecutor))]
    public class SelectCommandExecutor_Tests<TCommandExecutor> where TCommandExecutor : IDbCommandExecutor<IDataReader>, new()
    {
        static IDbCommandExecutor<IDataReader> executor = new TCommandExecutor();

        private static IEnumerable GetTables()
        {
            return Utils.TestModels.ToTables()
                .Select(t => new TestCaseData(t).Returns(Utils.RecordCounts[t.EntityType]));
        }

        [TestCaseSource("GetTables")]
        public int IDbCommandExecutor_SelectCommand_Assert(ITable table)
        {
            int count = 0;

            Utils.ExecuteWith(connection =>
            {
                var cmd = connection.CreateCommand();

                cmd.CommandText = table.SqlSelect;

                var reader = executor.ExecuteCommand(cmd);

                while (reader.Read())
                {
                    ++count;
                }
            });

            return count;
        }

    }
}
