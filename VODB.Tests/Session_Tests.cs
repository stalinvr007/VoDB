using NUnit.Framework;
using VODB.Tests.Models.Northwind;
using System.Linq;
using VODB.Core.Execution.Executers.DbResults;
using System.Collections;
using VODB.Sessions;
using VODB.DbLayer;
using VODB.EntityTranslation;
using VODB.EntityMapping;
using VODB.Sessions.EntityFactories;

namespace VODB.Tests
{
    [TestFixture]
    public class Session_Tests
    {
        private IEnumerable GetSessions()
        {
            yield return new SessionV1();

            yield return new SessionV2(
                new VodbConnection(Utils.ConnectionCreator),
                new EntityTranslator(),
                new OrderedEntityMapper(),
                new ProxyCreator()
            );

        }

        [TestCaseSource("GetSessions")]
        public void Session_GetAll(ISession session)
        {
            var employees = session.GetAll<Employee>();

            Assert.AreEqual(9, employees.Count());
        }

        [TestCaseSource("GetSessions")]
        public void Session_GetAll_Employees_From_Orders(ISession session)
        {
            var employees = session.GetAll<Employee>()
                .Where(e => e.ReportsTo).In(
                    session.GetAll<Orders>()
                );

            Assert.AreEqual(9, employees.Count());
        }

        [TestCaseSource("GetSessions")]
        public void Session_GetAll_Shippers_From_Orders(ISession session)
        {
            var shippers = session.GetAll<Shippers>().Where(e => e.ShipperId).In(
                session.GetAll<Orders>()
            );

            Assert.AreEqual(3, shippers.Count());
        }

        [TestCaseSource("GetSessions")]
        public void Session_GetAllOrders(ISession session)
        {
            var employees = session.GetAll<Orders>();

            Assert.AreEqual(830, employees.Count());
        }

        [TestCaseSource("GetSessions")]
        public void Session_WhereCondition_ShouldBeKept_ByResult(ISession session)
        {
            var query1 = session.GetAll<Employee>()
                .Where(m => m.EmployeeId > 4);

            var query2 = session.GetAll<Employee>()
                .Where(m => m.EmployeeId == 2);

            var query3 = session.GetAll<Employee>()
                .Where(m => m.EmployeeId == 2)
                .Or(m => m.EmployeeId == 3);


            Assert.AreEqual(5, query1.Count());
            Assert.AreEqual(1, query2.Count());
            Assert.AreEqual(2, query3.Count());

            Assert.AreEqual(5, query1.Count());
            Assert.AreEqual(1, query2.Count());
            Assert.AreEqual(2, query3.Count());

        }

        [TestCaseSource("GetSessions")]
        public void Session_GetAll_LikeCondition(ISession session)
        {
            var employees = session
                .GetAll<Employee>()
                .Where(e => e.LastName).Like("an");

            Assert.AreEqual(2, employees.Count());
        }

        [TestCaseSource("GetSessions")]
        public void Session_GetAll_LikeCondition_Left(ISession session)
        {
            var employees = session
                .GetAll<Employee>()
                .Where(e => e.LastName).Like("r", WildCard.Left);

            Assert.AreEqual(1, employees.Count());
        }

        [TestCaseSource("GetSessions")]
        public void Session_GetAll_OrderedByFirstName(ISession session)
        {
            var employee = session
                .GetAll<Employee>().OrderBy(e => e.FirstName)
                .First();

            EntitiesAsserts.Assert_Employee_2(employee);

        }

        [TestCaseSource("GetSessions")]
        public void Session_GetEmployeeList_ReportsToFilter(ISession session)
        {
            /* 
             * Select * from Employees Where ReportsTo in 
             *      (Select EmployeeId From Employees Where FirstName = 'Andrew')
             */
            var employees1 = session
                .GetAll<Employee>().Where(e => e.ReportsTo.FirstName == "Andrew");

            Assert.AreEqual(5, employees1.Count());

            EntitiesAsserts.Assert_Employee_1(employees1.First());

            /* 
             * Select * From Employees Where ReportsTo in 
             *      (Select EmployeeId From Employees Where EmployeeId = 1)
             */
            var employees2 = session
                .GetAll<Employee>().Where(e => e.ReportsTo.EmployeeId == 1);

            EntitiesAsserts.Assert_Employee_2(employees2.First());
        }

        [TestCaseSource("GetSessions")]
        public void Session_GetAll_OrderedByFirstName_TSql(ISession session)
        {
            var employee = session
                .GetAll<Employee>().OrderBy(e => e.FirstName)
                .First();

            EntitiesAsserts.Assert_Employee_2(employee);
        }

        [TestCaseSource("GetSessions")]
        public void Session_GetAll_OrderedByCity_Descending(ISession session)
        {
            var employee2 = session
                .GetAll<Employee>()
                .OrderBy(e => e.City)
                .Descending()
                .First();

            EntitiesAsserts.Assert_Employee_2(employee2);

            var employee3 = session
                .GetAll<Employee>()
                .OrderBy(e => e.City)
                .First();

            EntitiesAsserts.Assert_Employee_3(employee3);
        }


        [TestCaseSource("GetSessions")]
        public void QueryWith_OrCondition(ISession session)
        {
            var employees = session
                 .GetAll<Employee>()
                      .Where(m => m.EmployeeId == 1)
                      .Or(m => m.EmployeeId == 4)
                      .Or(m => m.EmployeeId == 2)
                      .Or(m => m.EmployeeId == 5)
                      .And(m => m.EmployeeId > 3);

            Assert.AreEqual(2, employees.Count());
        }

        [TestCaseSource("GetSessions")]
        public void MoreComplexQuery2(ISession session)
        {
            var employees = session
                 .GetAll<Employee>()
                      .Where(e => e.LastName).Like("r")
                      .Or(e => e.LastName).Like("a")
                 .OrderBy(m => m.City)
                 .Descending();

            Assert.AreEqual(8, employees.Count());
        }

        [TestCaseSource("GetSessions")]
        public void MoreComplexQuery(ISession session)
        {
            var employees = session
                 .GetAll<Employee>()
                      .Where(m => m.EmployeeId > 0)
                      .And(m => m.EmployeeId < 10)
                 .OrderBy(m => m.City)
                 .Descending();

            Assert.AreEqual(9, employees.Count());
        }

        [TestCaseSource("GetSessions")]
        public void Session_GetAll_withConditionExpression(ISession session)
        {
            var id = 1;
            var employees = session
                .GetAll<Employee>().Where(m => m.EmployeeId == id);

            Assert.AreEqual(1, employees.Count());
        }

        [TestCaseSource("GetSessions")]
        public void Session_GetAll_withInConditionExpression(ISession session)
        {
            var employees = session
                .GetAll<Employee>()
                    .Where(m => m.EmployeeId)
                    .In(new[] { 1, 2, 3 });

            Assert.AreEqual(3, employees.Count());
        }

        [TestCaseSource("GetSessions")]
        public void Session_GetAll_withInConditionExpression_Complex(ISession session)
        {
            var employees = session
                .GetAll<Employee>()
                    .Where(m => m.EmployeeId).In(new[] { 1, 2, 3, 4, 5, 6, 7 })
                    .And(m => m.EmployeeId >= 2);

            Assert.AreEqual(6, employees.Count());
        }

        [TestCaseSource("GetSessions")]
        public void Session_GetAll_Between(ISession session)
        {
            var employees = session
                .GetAll<Employee>()
                    .Where(m => m.EmployeeId)
                    .Between(1, 5);

            Assert.AreEqual(5, employees.Count());
        }

        [TestCaseSource("GetSessions")]
        public void Session_GetAll_In_using_collection(ISession session)
        {

            var collection = session.GetAll<Employee>()
                .Where(m => m.EmployeeId <= 5);

            var employees = session.GetAll<Employee>()
                .Where(m => m.EmployeeId)
                .In(collection);

            Assert.AreEqual(5, employees.Count());
        }

        [TestCaseSource("GetSessions")]
        public void Session_GetAll_In_using_collection_SameNames(ISession session)
        {

            var employee = session.GetById(new Employee { EmployeeId = 5 });
            
            /*
              Select * From Employees Where EmployeeId In (
                Select EmployeeId From Employees Where ReportsTo = @p1
              )
             */

            var employees = session.GetAll<Employee>()
                .Where(m => m.EmployeeId)
                .In(
                    session.GetAll<Employee>().Where(m => m.ReportsTo.EmployeeId == employee.EmployeeId)
                );
            
            Assert.AreEqual(3, employees.Count());
        }

        [TestCaseSource("GetSessions")]
        public void Session_GetAll_In_using_collection_diffNames(ISession session)
        {

            var orders = session.GetAll<Orders>()
                .Where(o => o.Shipper)
                .In(session.GetAll<Shippers>().Where(s => s.ShipperId == 2))
                .And(o => o.Employee.EmployeeId == 4);

            Assert.AreEqual(70, orders.Count());
        }

        [TestCaseSource("GetSessions")]
        public void Session_GetAll_withConditionExpression_IdConst(ISession session)
        {
            var employees = session
                .GetAll<Employee>().Where(m => m.EmployeeId == 1);

            Assert.AreEqual(1, employees.Count());
        }

        [TestCaseSource("GetSessions")]
        public void Session_Count(ISession session)
        {
            Assert.AreEqual(9, session.Count<Employee>());
        }

        [TestCaseSource("GetSessions")]
        public void Session_GetAll_withWhereCond(ISession session)
        {
            var employees = session
                .GetAll<Employee>()
                .Where("EmployeeId = {0}", 1);

            Assert.AreEqual(1, employees.Count());
        }

        [TestCaseSource("GetSessions")]
        public void Session_GetAll_withWhereCond_AndMoreConditions(ISession session)
        {
            var employees = session
                .GetAll<Employee>()
                .Where("FirstName = '{0}'", "Nancy")
                .And("LastName = '{0}'", "Davolio");

            Assert.AreEqual(1, employees.Count());
        }

        [TestCaseSource("GetSessions")]
        public void Session_GetById(ISession session)
        {
            var employee = session.GetById(new Employee { EmployeeId = 1 });

            EntitiesAsserts.Assert_Employee_1(employee);
        }

        [TestCaseSource("GetSessions")]
        public void Session_Exists(ISession session)
        {

            Assert.IsTrue(session.Exists(new Employee { EmployeeId = 1 }));
            Assert.IsTrue(session.Exists(new Employee { EmployeeId = 2 }));
            Assert.IsTrue(session.Exists(new Employee { EmployeeId = 3 }));

            Assert.IsFalse(session.Exists(new Employee { EmployeeId = 123123 }));

        }

        [TestCaseSource("GetSessions")]
        public void Session_GetById_ReportsTo(ISession session)
        {
            var employee = session.GetById(
                new Employee { EmployeeId = 1 });

            EntitiesAsserts.Assert_Employee_2(employee.ReportsTo);
            EntitiesAsserts.Assert_Employee_1(employee.ReportsTo.ReportsTo);
            EntitiesAsserts.Assert_Employee_2(employee.ReportsTo.ReportsTo.ReportsTo);
            EntitiesAsserts.Assert_Employee_1(employee);
        }

        [TestCaseSource("GetSessions")]
        public void Session_Insert_Employee(ISession session)
        {
            session.WithRollback(s =>
            {
                var count = session.GetAll<Employee>().Count();
                var employee = session
                    .Insert(new Employee
                    {
                        FirstName = "Sérgio",
                        LastName = "Ferreira"
                    });

                Assert.IsNotNull(employee);
                Assert.AreEqual(count + 1, session.GetAll<Employee>().Count());
            });

        }

        [TestCaseSource("GetSessions")]
        public void Session_Delete_Employee(ISession session)
        {

            session.WithRollback(s =>
                {
                    var count = session.GetAll<Employee>().Count();
                    var sergio = session.Insert(new Employee
                    {
                        FirstName = "Sérgio",
                        LastName = "Ferreira"
                    });

                    session.Delete(sergio);

                    Assert.AreEqual(count, session.GetAll<Employee>().Count());
                }
            );

        }

        [TestCaseSource("GetSessions")]
        public void Session_Update_Employee(ISession session)
        {

            session.WithRollback(s =>
                {
                    var sergio = session.Insert(new Employee
                    {
                        FirstName = "Sérgio",
                        LastName = "Ferreira"
                    });

                    sergio.FirstName = "Alien";

                    session.Update(sergio);

                    var alien = session.GetById(sergio);

                    Assert.That(alien, Is.Not.Null);
                    Assert.AreEqual("Alien", alien.FirstName);
                }
            );

        }
    }
}
