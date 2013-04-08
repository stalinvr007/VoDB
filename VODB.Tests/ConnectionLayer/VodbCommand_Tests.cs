using System;
using System.Collections;
using NUnit.Framework;
using VODB.DbLayer;
using VODB.ExpressionsToSql;
using VODB.Infrastructure;

namespace VODB.Tests.ConnectionLayer
{

    [TestFixture]
    public class VodbCommand_Tests
    {

        private TestCaseData Make(Func<IVodbCommandFactory, IVodbCommand> action)
        {
            return new TestCaseData(action);
        }

        private IEnumerable GetCommandsNonQuery()
        {
            yield return Make(v => v.MakeCommand("Insert Into Employees (FirstName, LastName) values ('testing', 'testing')"))
                .Returns(1)
                .SetName("VodbCommand Execute Insert employee");

            yield return Make(v => v.MakeCommand("Update Employees Set LastName = 'wow'"))
                .Returns(9)
                .SetName("VodbCommand Execute Update employees");

            yield return Make(v =>
                {
                    return v.MakeCommand("Update Employees Set LastName = @p2 where EmployeeId = @p1")
                        .SetParametersNames(new[] { "@p1", "@p2" })
                        .SetParametersValues(new[] { 
                            new QueryParameter { Value = 1 , Field = new Field("EmployeeId", typeof(int), null, null)}, 
                            new QueryParameter { Value = "wow" , Field = new Field("LastName", typeof(string), null, null) }
                        });
                })
                .Returns(1)
                .SetName("VodbCommand Execute Update employees with parameters");
        }

        [TestCaseSource("GetCommandsNonQuery")]
        public int VodbCommand_Execution_AssertNonQuery(Func<IVodbCommandFactory, IVodbCommand> makeCommand)
        {
            var result = 0;
            using (var connection = new VodbConnection(Utils.ConnectionCreator))
            {
                connection.WithRollback(c =>
                {
                    result = connection.ExecuteNonQuery(makeCommand(connection));
                });
            }
            return result;
        }

        [Test, ExpectedException(typeof(InvalidOperationException))]
        public void VodbCommand_Execution_Failed_ConnectionClosed()
        {
            using (var connection = new VodbConnection(Utils.ConnectionCreator))
            {
                var command = connection.MakeCommand("Insert Into Employees (FirstName, LastName) values ('testing', 'testing')");
                command.ExecuteNonQuery();
            }
        }

    }

}
