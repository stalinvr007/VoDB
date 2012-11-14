using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VODB.Tests.Models.Northwind;

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
        public void EagerSession_GetById()
        {
            var employee = SessionsFactory.CreateEager().GetById(
                new Employee { EmployeeId = 1 });

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
    }
}