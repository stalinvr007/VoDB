﻿using NUnit.Framework;
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
        private const string SELECT_EMPLOYEES = "Select [EmployeeId], [LastName], [FirstName], [Title], [TitleOfCourtesy], [BirthDate], [HireDate], [Address], [City], [Region], [PostalCode], [Country], [HomePhone], [Extension], [Notes], [Photo], [ReportsTo], [PhotoPath] From [Employees]";

        private static IEntityTranslator _Translator = new EntityTranslator();

        private static TestCaseData MakeTestCase(IQueryCondition query, int paramCount = 0)
        {
            return new TestCaseData(query, paramCount);
        }
        private IEnumerable GetEmployeeQueries()
        {
            yield return MakeTestCase(

                Select.All.From<Employee>().Where(e => e.EmployeeId > Param.Get<int>()), 1

            ).Returns(SELECT_EMPLOYEES + " Where [EmployeeId] > @p1")
            .SetName("Query employee (Where EmployeeId > @p1)");

            yield return MakeTestCase(

                Select.All.From<Employee>().Where(e => e.ReportsTo == Param.Get<Employee>()), 1

            ).Returns(SELECT_EMPLOYEES + " Where [ReportsTo] = @p1")
            .SetName("Query employee (Where Reports = @p1)");

            yield return MakeTestCase(

                Select.All.From<Employee>().Where(e => e.ReportsTo.EmployeeId > Param.Get<int>()), 1

            ).Returns(SELECT_EMPLOYEES + " Where [ReportsTo] in (Select [EmployeeId] From [Employees] Where [EmployeeId] > @p1)")
            .SetName("Query employee (Where ReportsTo > @p1)");

            yield return MakeTestCase(

                Select.All.From<Employee>().Where(e => e.ReportsTo.LastName == Param.Get<String>()), 1

            ).Returns(SELECT_EMPLOYEES + " Where [ReportsTo] in (Select [EmployeeId] From [Employees] Where [LastName] = @p1)")
            .SetName("Query employee (Where ReportsTo.LastName = @p1)");

            yield return MakeTestCase(

                Select.All.From<Employee>().Where(e => e.EmployeeId > Param.Get<int>())
                    .OrderBy(e => e.ReportsTo)
                    .Descending(), 1

            ).Returns(SELECT_EMPLOYEES + " Where [EmployeeId] > @p1 Order By [ReportsTo] Desc")
            .SetName("Query employee (Where EmployeeId > @p1 with order desc)");

            yield return MakeTestCase(

                Select.All.From<Employee>()
                        .Where(e => e.EmployeeId > Param.Get<int>())
                        .And(e => e.EmployeeId < Param.Get<int>())
                    .OrderBy(e => e.City)
                    .Descending(), 2

            ).Returns(SELECT_EMPLOYEES + " Where [EmployeeId] > @p1 And [EmployeeId] < @p2 Order By [City] Desc")
            .SetName("Query employee (Where EmployeeId > @p1 And EmployeeId < @p2 with order desc)");

            yield return MakeTestCase(

                Select.All.From<Employee>()
                        .Where(e => e.EmployeeId > Param.Get<int>())
                        .And(e => e.EmployeeId < Param.Get<int>())
                        .And(e => e.EmployeeId <= Param.Get<int>())
                        .And(e => e.EmployeeId >= Param.Get<int>())
                        .And(e => e.EmployeeId == Param.Get<int>())
                        .And(e => e.EmployeeId != Param.Get<int>())
                    .OrderBy(e => e.City)
                    .Descending(), 6

            ).Returns(SELECT_EMPLOYEES + " Where [EmployeeId] > @p1 And [EmployeeId] < @p2 And [EmployeeId] <= @p3 And [EmployeeId] >= @p4 And [EmployeeId] = @p5 And [EmployeeId] != @p6 Order By [City] Desc")
            .SetName("Query employee (Where EmployeeId all comparations with order desc)");

            yield return MakeTestCase(

                Select.All.From<Employee>().Where(e => e.ReportsTo.EmployeeId > Param.Get<int>())
                    .OrderBy(e => e.Region), 1

            ).Returns(SELECT_EMPLOYEES + " Where [ReportsTo] in (Select [EmployeeId] From [Employees] Where [EmployeeId] > @p1) Order By [Region]")
            .SetName("Query employee (Where ReportsTo > @p1 order by region)");

            yield return MakeTestCase(

                Select.All.From<Employee>().Where(e => e.ReportsTo.EmployeeId > Param.Get<int>())
                    .OrderBy(e => e.ReportsTo.EmployeeId), 1

             ).Throws(typeof(OrderByClauseException))
             .SetName("Query employee (order by invalid)");

            yield return MakeTestCase(

                Select.All.From<Employee>().Where(e => e.EmployeeId > Param.Get<int>())
                    .And(e => e.Title == Param.Get<String>()), 2

            ).Returns(SELECT_EMPLOYEES + " Where [EmployeeId] > @p1 And [Title] = @p2")
            .SetName("Query employee (Where EmployeeId > @p1 and Title = @p2)");

            yield return MakeTestCase(

                Select.All.From<Employee>().Where(e => e.EmployeeId > Param.Get<int>())
                    .And(e => e.Title == Param.Get<String>())
                    .And(e => e.TitleOfCourtesy == Param.Get<String>()), 3

            ).Returns(SELECT_EMPLOYEES + " Where [EmployeeId] > @p1 And [Title] = @p2 And [TitleOfCourtesy] = @p3")
            .SetName("Query employee (Where EmployeeId > @p1 and Title = @p2)");

            yield return MakeTestCase(

                Select.All.From<Employee>().Where(e => e.EmployeeId > Param.Get<int>())
                    .And(e => e.Title == Param.Get<String>())
                    .Or(e => e.TitleOfCourtesy == Param.Get<String>()), 3

            ).Returns(SELECT_EMPLOYEES + " Where [EmployeeId] > @p1 And ([Title] = @p2 Or [TitleOfCourtesy] = @p3)")
            .SetName("Query employee (Where EmployeeId > @p1 and Title = @p2 or TitleOfCourtesy = @p3)");

            yield return MakeTestCase(

                Select.All.From<Employee>().Where(e => e.EmployeeId).Between(Param.Get<int>(), Param.Get<int>()), 2

            ).Returns(SELECT_EMPLOYEES + " Where [EmployeeId] Between @p1 And @p2")
            .SetName("Query employee (Where EmployeeId Between)");

            yield return MakeTestCase(

                Select.All.From<Employee>().Where(e => e.ReportsTo.EmployeeId).Between(Param.Get<int>(), Param.Get<int>()), 2

            ).Returns(SELECT_EMPLOYEES + " Where [ReportsTo] in (Select [EmployeeId] From [Employees] Where [EmployeeId] Between @p1 And @p2)")
            .SetName("Query employee (Where ReportsTo Between)");

            yield return MakeTestCase(

                Select.All.From<Employee>().Where(e => e.EmployeeId).Like(Param.Get<String>()), 1

            ).Returns(SELECT_EMPLOYEES + " Where [EmployeeId] Like '%' + @p1 + '%'")
            .SetName("Query employee (Where EmployeeId Like)");

            yield return MakeTestCase(

                Select.All.From<Employee>().Where(e => e.EmployeeId).In(new Object[] { 1, 2, 3 }), 3

            ).Returns(SELECT_EMPLOYEES + " Where [EmployeeId] In (@p1, @p2, @p3)")
            .SetName("Query employee (Where EmployeeId in)");

            yield return MakeTestCase(

                Select.All.From<Employee>().Where(e => e.EmployeeId).Between(Param.Get<int>(), Param.Get<int>())
                    .Or(e => e.EmployeeId).Between(Param.Get<int>(), Param.Get<int>()), 4

            ).Returns(SELECT_EMPLOYEES + " Where ([EmployeeId] Between @p1 And @p2 Or [EmployeeId] Between @p3 And @p4)")
            .SetName("Query employee (Where EmployeeId between and or condition)");

            yield return MakeTestCase(

                Select.All.From<Employee>().Where(e => e.EmployeeId > Param.Get<int>())
                    .Or(e => e.EmployeeId == Param.Get<int>())
                    .Or(e => e.EmployeeId == Param.Get<int>()), 3

            ).Returns(SELECT_EMPLOYEES + " Where ([EmployeeId] > @p1 Or [EmployeeId] = @p2 Or [EmployeeId] = @p3)")
            .SetName("Query employee (Where EmployeeId or condition)");

            yield return MakeTestCase(

                Select.All.From<Employee>().Where(e => e.EmployeeId > Param.Get<int>())
                    .Or(e => e.EmployeeId == Param.Get<int>())
                    .Or(e => e.EmployeeId == Param.Get<int>())
                    .Or(e => e.EmployeeId == Param.Get<int>()), 4

            ).Returns(SELECT_EMPLOYEES + " Where ([EmployeeId] > @p1 Or [EmployeeId] = @p2 Or [EmployeeId] = @p3 Or [EmployeeId] = @p4)")
            .SetName("Query employee (Where EmployeeId or conditions)");

            yield return MakeTestCase(

                Select.All.From<Employee>().Where(e => e.EmployeeId > Param.Get<int>())
                    .Or(e => e.EmployeeId == Param.Get<int>())
                    .And(e => e.EmployeeId == Param.Get<int>())
                    .Or(e => e.EmployeeId == Param.Get<int>()), 4

            ).Returns(SELECT_EMPLOYEES + " Where ([EmployeeId] > @p1 Or [EmployeeId] = @p2) And ([EmployeeId] = @p3 Or [EmployeeId] = @p4)")
            .SetName("Query employee (Where EmployeeId And Or conditions)");

            yield return MakeTestCase(

                Select.All.From<Employee>().Where(e => e.EmployeeId > Param.Get<int>())
                    .Or(e => e.EmployeeId == Param.Get<int>())
                    .Or(e => e.EmployeeId == Param.Get<int>())
                    .Or(e => e.EmployeeId).Between(Param.Get<int>(), Param.Get<int>()), 5

            ).Returns(SELECT_EMPLOYEES + " Where ([EmployeeId] > @p1 Or [EmployeeId] = @p2 Or [EmployeeId] = @p3 Or [EmployeeId] Between @p4 And @p5)")
            .SetName("Query employee (Where EmployeeId between and or condition)");

            yield return MakeTestCase(

                Select.All.From<Orders>().Where(e => e.Shipper).In(
                    Select.All.From<Shippers>().Where(z => z.CompanyName == Param.Get<String>())
                ), 1

            ).Returns("Select [OrderId], [CustomerId], [EmployeeId], [OrderDate], [RequiredDate], [ShippedDate], [ShipVia], [Freight], [ShipName], [ShipAddress], [ShipCity], [ShipRegion], [ShipPostalCode], [ShipCountry] From [Orders] Where [ShipVia] In (Select [ShipperId] From [Shippers] Where [CompanyName] = @p1)")
            .SetName("Query employee (Where EmployeeId in SubQuery)");

            yield return MakeTestCase(

                Select.All.From<Orders>().Where(e => e.Shipper).In(
                    Select.All.From<Shippers>().Where(z => z.CompanyName == Param.Get<String>())
                ).And(e => e.OrderDate == Param.Get<DateTime>()) , 2

            ).Returns("Select [OrderId], [CustomerId], [EmployeeId], [OrderDate], [RequiredDate], [ShippedDate], [ShipVia], [Freight], [ShipName], [ShipAddress], [ShipCity], [ShipRegion], [ShipPostalCode], [ShipCountry] From [Orders] Where [ShipVia] In (Select [ShipperId] From [Shippers] Where [CompanyName] = @p1) And [OrderDate] = @p2")
            .SetName("Query employee (Where EmployeeId in SubQuery multiple params)");

            yield return MakeTestCase(

                Select.All.From<Employee>()

            ).Returns(SELECT_EMPLOYEES)
            .SetName("Select All From Employee");
            
        }

        [TestCaseSource("GetEmployeeQueries")]
        public String QueryCompiler_Assert_Result_Employee(IQueryCondition query, int paramCount)
        {
            var result = query.Compile();
            Assert.That(((IList<IQueryParameter>)query.Parameters).Count, Is.EqualTo(paramCount));
            return result;
        }

    }
}
