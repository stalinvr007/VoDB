using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using VODB.EntityTranslation;
using VODB.Exceptions;
using VODB.QueryCompiler;
using VODB.Tests.Models.Northwind;

namespace VODB.Tests.QueryCompiler
{
    [TestFixture]
    public class QueryCompiler_Tests
    {

        private static IEntityTranslator _Translator = new EntityTranslator();

        private static TestCaseData MakeTestCase<TEntity>(Func<IQueryCompilerLevel1<TEntity>, IEnumerable<TEntity>> func) where TEntity : class, new()
        {
            return new TestCaseData(Query<TEntity>.PreCompile_QueryCompiler(func));
        }
        private IEnumerable GetEmployeeQueries()
        {
            yield return MakeTestCase<Employee>(query => 
                
                query.Where(e => e.EmployeeId > Param.Get<int>())
            
            ).Returns(" Where [EmployeeId] > @p1")
            .SetName("Query employee (Where EmployeeId > @p1)");

            yield return MakeTestCase<Employee>(query => 
                
                query.Where(e => e.ReportsTo.EmployeeId > Param.Get<int>())
                
            ).Returns(" Where [EmployeeId] in (Select [EmployeeId] From [Employees] Where [ReportsTo] > @p1)")
            .SetName("Query employee (Where ReportsTo > @p1)");

            yield return MakeTestCase<Employee>(query =>

                query.Where(e => e.EmployeeId > Param.Get<int>())
                    .OrderBy(e => e.ReportsTo)
                    .Descending()

            ).Returns(" Where [EmployeeId] > @p1 Order By [ReportsTo] Desc")
            .SetName("Query employee (Where EmployeeId > @p1 with order desc)");

            yield return MakeTestCase<Employee>(query =>

                query.Where(e => e.ReportsTo.EmployeeId > Param.Get<int>())
                    .OrderBy(e => e.Region)

            ).Returns(" Where [EmployeeId] in (Select [EmployeeId] From [Employees] Where [ReportsTo] > @p1) Order By [Region]")
            .SetName("Query employee (Where ReportsTo > @p1 order by region)");

            yield return MakeTestCase<Employee>(query => 

                query.Where(e => e.ReportsTo.EmployeeId > Param.Get<int>())
                    .OrderBy(e => e.ReportsTo.EmployeeId)
             
             ).Returns("Doesn't matter")
            .SetName("Query employee (order by invalid)")
            .Throws(typeof(OrderByClauseException));
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
