using NUnit.Framework;
using VODB.Tests.Models.Northwind;

namespace VODB.Tests.QueryCompiler
{
    [TestFixture]
    public class QueryCompiler_Tests
    {

        public void QueryCompiler_Assert_Result()
        {
            Query<Employee>.PreCompile_QueryCompiler(result => result.Where(f => f.EmployeeId == 1));
        }

    }
}
