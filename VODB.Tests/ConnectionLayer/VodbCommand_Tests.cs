using System;
using System.Collections;
using NUnit.Framework;
using VODB.DbLayer;

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
                        .SetParametersNames("@p1", "@p2")
                        .SetParametersValues(1, "wow");
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
