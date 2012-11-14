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
            var employee = SessionsFactory.CreateEager().GetById<Employee>(
                new Employee { EmployeeId = 1 });

            EntitiesAsserts.Assert_Employee_1(employee);
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
            var task1 = SessionsFactory.CreateEager().AsyncGetAll<Employee>();
            var task2 = SessionsFactory.CreateEager().AsyncGetAll<Employee>();
            var task3 = SessionsFactory.CreateEager().AsyncGetAll<Employee>();

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