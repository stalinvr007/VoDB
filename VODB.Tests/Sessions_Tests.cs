using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VODB.Tests.Models.Northwind;
using VODB.Sessions;

namespace VODB.Tests
{
    [TestClass]
    public class Sessions_Tests
    {
        [TestMethod]
        public void EagerSession_GetAll()
        {
            var employees = SessionsFactory.CreateEager().GetAll<Employee>();

            Assert.AreEqual(9, employees.Count());
        }

        [TestMethod]
        public void EagerSession_GetAll_OrderedByFirstName()
        {
            var employee = SessionsFactory.CreateEager()
                .GetAll<Employee>().OrderBy(e => e.FirstName)
                .First();

            EntitiesAsserts.Assert_Employee_2(employee);

        }

        [TestMethod]
        public void EagerSession_GetAll_OrderedByCity_Descending()
        {
            var employee2 = SessionsFactory.CreateEager()
                .GetAll<Employee>()
                .OrderBy(e => e.City)
                .Descending()
                .First();

            EntitiesAsserts.Assert_Employee_2(employee2);

            var employee3 = SessionsFactory.CreateEager()
                .GetAll<Employee>()
                .OrderBy(e => e.City)
                .First();

            EntitiesAsserts.Assert_Employee_3(employee3);

        }

        [TestMethod]
        public void MoreComplexQuery()
        {
            var employees = SessionsFactory.CreateEager()
                 .GetAll<Employee>()
                      .Where(m => m.EmployeeId > 0)
                      .And(m => m.EmployeeId < 10)
                 .OrderBy(m => m.City)
                 .Descending();

            Assert.AreEqual(9, employees.Count());
        }

        [TestMethod]
        public void EagerSession_GetAll_withConditionExpression()
        {
            var id = 1;
            var employees = SessionsFactory.CreateEager()
                .GetAll<Employee>().Where(m => m.EmployeeId == id);

            Assert.AreEqual(1, employees.Count());
        }

        [TestMethod]
        public void EagerSession_GetAll_withInConditionExpression()
        {
            var employees = SessionsFactory.CreateEager()
                .GetAll<Employee>()
                    .Where(m => m.EmployeeId)
                    .In(new[] { 1, 2, 3 });

            Assert.AreEqual(3, employees.Count());
        }

        [TestMethod]
        public void EagerSession_GetAll_withInConditionExpression_Complex()
        {
            var employees = SessionsFactory.CreateEager()
                .GetAll<Employee>()
                    .Where(m => m.EmployeeId).In(new[] { 1, 2, 3, 4, 5, 6, 7 })
                    .And(m => m.EmployeeId >= 2);

            Assert.AreEqual(6, employees.Count());
        }

        [TestMethod]
        public void EagerSession_GetAll_Between()
        {
            var employees = SessionsFactory.CreateEager()
                .GetAll<Employee>()
                    .Where(m => m.EmployeeId)
                    .Between(1, 5);

            Assert.AreEqual(5, employees.Count());
        }

        [TestMethod]
        public void EagerSession_GetAll_In_using_collection()
        {
            ISession session = new EagerSession();
            var collection = session.GetAll<Employee>()
                .Where(m => m.EmployeeId <= 5);

            var employees = session.GetAll<Employee>()
                .Where(m => m.EmployeeId)
                .In<Employee>(collection);

            Assert.AreEqual(5, employees.Count());
        }

        [TestMethod]
        public void EagerSession_GetAll_In_using_collection_SameNames()
        {
            ISession session = new EagerSession();

            var employee = session.GetById(new Employee { EmployeeId = 5 });

            var employees = session.GetAll<Employee>()
                .Where(m => m.EmployeeId)
                .In<Employee>(employee.ReportedFrom);

            Assert.AreEqual(3, employees.Count());
        }

        [TestMethod]
        public void EagerSession_GetAll_In_using_collection_diffNames()
        {
            ISession session = new EagerSession();

            Employee employee4 = new Employee { EmployeeId = 4 };
            var orders = session.GetAll<Orders>()
                .Where(o => o.Shipper)
                .In(session.GetAll<Shippers>().Where(s => s.ShipperId == 2))
                .And(o => o.Employee == employee4);

            Assert.AreEqual(70, orders.Count());
        }

        [TestMethod]
        public void EagerSession_GetAll_withConditionExpression_IdConst()
        {
            var employees = SessionsFactory.CreateEager()
                .GetAll<Employee>().Where(m => m.EmployeeId == 1);

            Assert.AreEqual(1, employees.Count());
        }

        [TestMethod]
        public void EagerSession_Count()
        {
            Assert.AreEqual(9, SessionsFactory.CreateEager().Count<Employee>());
        }

        [TestMethod]
        public void EagerSession_GetAll_withWhereCond()
        {
            var employees = SessionsFactory.CreateEager()
                .GetAll<Employee>()
                .Where("EmployeeId = {0}", 1);

            Assert.AreEqual(1, employees.Count());
        }

        [TestMethod]
        public void EagerSession_GetAll_withWhereCond_AndMoreConditions()
        {
            var employees = SessionsFactory.CreateEager()
                .GetAll<Employee>()
                .Where("FirstName = '{0}'", "Nancy")
                .And("LastName = '{0}'", "Davolio");

            Assert.AreEqual(1, employees.Count());
        }

        [TestMethod]
        public void EagerSession_GetById()
        {
            var employee = SessionsFactory.CreateEager().GetById(
                new Employee { EmployeeId = 1 });

            EntitiesAsserts.Assert_Employee_1(employee);
        }

        [TestMethod]
        public void EagerSession_Exists()
        {
            using (var session = new EagerSession())
            {
                Assert.IsTrue(session.Exists(new Employee { EmployeeId = 1 }));
                Assert.IsTrue(session.Exists(new Employee { EmployeeId = 2 }));
                Assert.IsTrue(session.Exists(new Employee { EmployeeId = 3 }));

                Assert.IsFalse(session.Exists(new Employee { EmployeeId = 123123 }));
            }
        }

        [TestMethod]
        public void EagerSession_GetById_ReportsFrom()
        {
            var employee = SessionsFactory.CreateEager().GetById(
                new Employee { EmployeeId = 1 });

            Assert.AreEqual(1, employee.ReportedFrom.Count());

            EntitiesAsserts.Assert_Employee_1(employee);
        }

        [TestMethod]
        public void EagerSession_GetById_ReportsTo()
        {
            var employee = SessionsFactory.CreateEager().GetById(
                new Employee { EmployeeId = 1 });

            EntitiesAsserts.Assert_Employee_2(employee.ReportsTo);
            EntitiesAsserts.Assert_Employee_1(employee.ReportsTo.ReportsTo);
            EntitiesAsserts.Assert_Employee_2(employee.ReportsTo.ReportsTo.ReportsTo);
            EntitiesAsserts.Assert_Employee_1(employee);
        }

        [TestMethod]
        public void EagerSession_AsyncGetById()
        {
            var task = SessionsFactory.CreateEager().AsyncGetById(
                new Employee { EmployeeId = 1 });

            /* Simulate some work... */
            Thread.Sleep(50);

            var employee = task.Result;

            EntitiesAsserts.Assert_Employee_1(employee);
        }

        [TestMethod]
        public void EagerSession_AsyncGetById_multipleCalls()
        {
            var session = SessionsFactory.CreateEager();

            var task1 = session.AsyncGetById(new Employee { EmployeeId = 1 });
            var task2 = session.AsyncGetById(new Employee { EmployeeId = 2 });
            var task3 = session.AsyncGetById(new Employee { EmployeeId = 3 });


            /* Simulate some work... */
            Thread.Sleep(50);

            var employee1 = task1.Result;
            var employee2 = task2.Result;
            var employee3 = task3.Result;

            EntitiesAsserts.Assert_Employee_1(employee1);
            EntitiesAsserts.Assert_Employee_2(employee2);
            EntitiesAsserts.Assert_Employee_3(employee3);
        }

        [TestMethod]
        public void EagerSession_AsyncGetAll()
        {
            var task = SessionsFactory.CreateEager().AsyncGetAll<Employee>();

            /* Simulate some work... */
            Thread.Sleep(50);

            var employees = task.Result;

            Assert.AreEqual(9, employees.Count());
        }

        [TestMethod]
        public void EagerSession_AsyncGetAll_multipleCalls()
        {
            var session = SessionsFactory.CreateEager();
            var task1 = session.AsyncGetAll<Employee>();
            var task2 = session.AsyncGetAll<Employee>();
            var task3 = session.AsyncGetAll<Employee>();

            /* Simulate some work... */
            Thread.Sleep(50);

            var employees1 = task1.Result;
            var employees2 = task2.Result;
            var employees3 = task3.Result;

            Assert.AreEqual(9, employees1.Count());
            Assert.AreEqual(9, employees2.Count());
            Assert.AreEqual(9, employees3.Count());
        }

        [TestMethod]
        public void EagerSession_Insert_Employee()
        {
            Utils.EagerExecuteWithinTransaction(session =>
            {
                var employee = session
                    .Insert(new Employee
                    {
                        FirstName = "Sérgio",
                        LastName = "Ferreira"
                    });

                Assert.IsNotNull(employee);
            });

        }

        [TestMethod]
        public void EagerSession_Delete_Employee()
        {


            Utils.EagerExecuteWithinTransaction(
                session =>
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

        [TestMethod]
        public void EagerSession_Update_Employee()
        {

            Utils.EagerExecuteWithinTransaction(
                session =>
                {
                    var sergio = session.Insert(new Employee
                    {
                        FirstName = "Sérgio",
                        LastName = "Ferreira"
                    });

                    sergio.FirstName = "Alien";

                    session.Update(sergio);

                    var alien = session.GetById(sergio);

                    Assert.AreEqual("Alien", alien.FirstName);
                }
            );

        }
    }
}