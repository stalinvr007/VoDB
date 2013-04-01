using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using VODB.EntityTranslation;
using VODB.Exceptions;
using VODB.ExpressionsToSql;
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

            ).Returns(" Where [ReportsTo] in (Select [EmployeeId] From [Employees] Where [EmployeeId] > @p1)")
            .SetName("Query employee (Where ReportsTo > @p1)");

            yield return MakeTestCase<Employee>(query =>

                query.Where(e => e.ReportsTo.LastName == Param.Get<String>())

            ).Returns(" Where [ReportsTo] in (Select [EmployeeId] From [Employees] Where [LastName] = @p1)")
            .SetName("Query employee (Where ReportsTo.LastName = @p1)");

            yield return MakeTestCase<Employee>(query =>

                query.Where(e => e.EmployeeId > Param.Get<int>())
                    .OrderBy(e => e.ReportsTo)
                    .Descending()

            ).Returns(" Where [EmployeeId] > @p1 Order By [ReportsTo] Desc")
            .SetName("Query employee (Where EmployeeId > @p1 with order desc)");

            yield return MakeTestCase<Employee>(query =>

                query.Where(e => e.ReportsTo.EmployeeId > Param.Get<int>())
                    .OrderBy(e => e.Region)

            ).Returns(" Where [ReportsTo] in (Select [EmployeeId] From [Employees] Where [EmployeeId] > @p1) Order By [Region]")
            .SetName("Query employee (Where ReportsTo > @p1 order by region)");

            yield return MakeTestCase<Employee>(query =>

                query.Where(e => e.ReportsTo.EmployeeId > Param.Get<int>())
                    .OrderBy(e => e.ReportsTo.EmployeeId)

             ).Returns("Doesn't matter")
            .SetName("Query employee (order by invalid)")
            .Throws(typeof(OrderByClauseException));

            yield return MakeTestCase<Employee>(query =>

                query.Where(e => e.EmployeeId > Param.Get<int>())
                    .And(e => e.Title == Param.Get<String>())

            ).Returns(" Where [EmployeeId] > @p1 And [Title] = @p2")
            .SetName("Query employee (Where EmployeeId > @p1 and Title = @p2)");

            yield return MakeTestCase<Employee>(query =>

                query.Where(e => e.EmployeeId > Param.Get<int>())
                    .And(e => e.Title == Param.Get<String>())
                    .And(e => e.TitleOfCourtesy == Param.Get<String>())

            ).Returns(" Where [EmployeeId] > @p1 And [Title] = @p2 And [TitleOfCourtesy] = @p3")
            .SetName("Query employee (Where EmployeeId > @p1 and Title = @p2)");

            yield return MakeTestCase<Employee>(query =>

                query.Where(e => e.EmployeeId > Param.Get<int>())
                    .And(e => e.Title == Param.Get<String>())
                    .Or(e => e.TitleOfCourtesy == Param.Get<String>())

            ).Returns(" Where [EmployeeId] > @p1 And ([Title] = @p2 Or [TitleOfCourtesy] = @p3)")
            .SetName("Query employee (Where EmployeeId > @p1 or Title = @p2)");

            yield return MakeTestCase<Employee>(query =>

                query.Where(e => e.EmployeeId).Between(Param.Get<int>(), Param.Get<int>())

            ).Returns(" Where [EmployeeId] Between @p1 And @p2")
            .SetName("Query employee (Where EmployeeId Between)");

            yield return MakeTestCase<Employee>(query =>

                query.Where(e => e.ReportsTo.EmployeeId).Between(Param.Get<int>(), Param.Get<int>())

            ).Returns(" Where [ReportsTo] in (Select [EmployeeId] From [Employees] Where [EmployeeId] Between @p1 And @p2)")
            .SetName("Query employee (Where ReportsTo Between)");

            yield return MakeTestCase<Employee>(query =>

                query.Where(e => e.EmployeeId).Like(Param.Get<String>())

            ).Returns(" Where [EmployeeId] Like '%' + @p1 + '%'")
            .SetName("Query employee (Where EmployeeId Like)");

            yield return MakeTestCase<Employee>(query =>

                query.Where(e => e.EmployeeId).In(new Object[] { 1, 2, 3 })

            ).Returns(" Where [EmployeeId] In (@p1, @p2, @p3)")
            .SetName("Query employee (Where EmployeeId in)");

            yield return MakeTestCase<Employee>(query =>

                query.Where(e => e.EmployeeId).Between(Param.Get<int>(), Param.Get<int>())
                    .Or(e => e.EmployeeId).Between(Param.Get<int>(), Param.Get<int>())

            ).Returns(" Where ([EmployeeId] Between @p1 And @p2 Or [EmployeeId] Between @p3 And @p4)")
            .SetName("Query employee (Where EmployeeId between and or condition)");

            yield return MakeTestCase<Employee>(query =>

                query.Where(e => e.EmployeeId > Param.Get<int>())
                    .Or(e => e.EmployeeId == Param.Get<int>())
                    .Or(e => e.EmployeeId == Param.Get<int>())

            ).Returns(" Where ([EmployeeId] > @p1 Or [EmployeeId] = @p2 Or [EmployeeId] = @p3)")
            .SetName("Query employee (Where EmployeeId or condition)");

            yield return MakeTestCase<Employee>(query =>

                query.Where(e => e.EmployeeId > Param.Get<int>())
                    .Or(e => e.EmployeeId == Param.Get<int>())
                    .Or(e => e.EmployeeId == Param.Get<int>())
                    .Or(e => e.EmployeeId == Param.Get<int>())

            ).Returns(" Where ([EmployeeId] > @p1 Or [EmployeeId] = @p2 Or [EmployeeId] = @p3 Or [EmployeeId] = @p4)")
            .SetName("Query employee (Where EmployeeId or conditions)");

            yield return MakeTestCase<Employee>(query =>

                query.Where(e => e.EmployeeId > Param.Get<int>())
                    .Or(e => e.EmployeeId == Param.Get<int>())
                    .And(e => e.EmployeeId == Param.Get<int>())
                    .Or(e => e.EmployeeId == Param.Get<int>())

            ).Returns(" Where ([EmployeeId] > @p1 Or [EmployeeId] = @p2) And ([EmployeeId] = @p3 Or [EmployeeId] = @p4)")
            .SetName("Query employee (Where EmployeeId And Or conditions)");

            yield return MakeTestCase<Employee>(query =>

                query.Where(e => e.EmployeeId > Param.Get<int>())
                    .Or(e => e.EmployeeId == Param.Get<int>())
                    .Or(e => e.EmployeeId == Param.Get<int>())
                    .Or(e => e.EmployeeId).Between(Param.Get<int>(), Param.Get<int>())

            ).Returns(" Where ([EmployeeId] > @p1 Or [EmployeeId] = @p2 Or [EmployeeId] = @p3 Or [EmployeeId] Between @p4 And @p5)")
            .SetName("Query employee (Where EmployeeId between and or condition)");
        }

        [TestCaseSource("GetEmployeeQueries")]
        public String QueryCompiler_Assert_Result_Employee(IQueryCondition query)
        {
            int level = 0;
            var result = query.Compile(ref level);

            string select = _Translator.Translate(typeof(Employee)).SqlSelect;
            StringAssert.StartsWith(select, result);
            Assert.That(query.Parameters.GetEnumerator().MoveNext(), Is.True);

            return result.Remove(0, select.Length);
        }

    }
}
