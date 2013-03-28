using NUnit.Framework;
using System;
using System.Collections;
using VODB.EntityTranslation;
using VODB.Tests.Models.Northwind;

namespace VODB.Tests.QueryCompiler
{
    [TestFixture]
    public class QueryCompiler_Tests
    {

        private static IEntityTranslator _Translator = new EntityTranslator();

        private IEnumerable GetEmployeeQueries()
        {
            yield return new TestCaseData(Query<Employee>.PreCompile_QueryCompiler(query =>
                query.Where(e => e.EmployeeId > Param.Get<int>())
            )).Returns("EmployeeId > @p1");
        }

        [TestCaseSource("GetEmployeeQueries")]
        public String QueryCompiler_Assert_Result_Employee(IQuery<Employee> query)
        {
            int level = 0;
            var result = query.Compile(ref level);

            string select = _Translator.Translate(typeof(Employee)).SqlSelect;
            StringAssert.StartsWith(select, result);
            return result.Remove(0, select.Length);
        }

    }
}
