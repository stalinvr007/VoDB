using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VODB.DbLayer;

namespace VODB.Tests.ConnectionLayer
{
    
    [TestFixture]
    public class VodbCommand_Tests
    {

        [TestCaseSource("GetCommandsNonQuery")]
        public void VodbCommand_Execution_AssertNonQuery(IVodbCommand command)
        {

        }

    }

}
