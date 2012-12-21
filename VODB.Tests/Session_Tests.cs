using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VODB.Tests.Models.Northwind;
using System.Linq;
using System.Threading;
using VODB.Core.Execution.Executers.DbResults;

namespace VODB.Tests
{
    [TestClass]
    public class Session_Tests
    {
        [TestMethod]
        public void Session_GetAll()
        {
            var employees = new Session().GetAll<Employee>();

            Assert.AreEqual(9, employees.Count());
        }

        [TestMethod]
        public void Session_WhereCondition_ShouldBeKept_ByResult()
        {
            var session = new Session();
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

        [TestMethod]
        public void Session_GetAll_LikeCondition()
        {
            var employees = new Session()
                .GetAll<Employee>()
                .Where(e => e.LastName).Like("an");

            Assert.AreEqual(2, employees.Count());
        }

        [TestMethod]
        public void Session_GetAll_LikeCondition_Left()
        {
            var employees = new Session()
                .GetAll<Employee>()
                .Where(e => e.LastName).Like("r", WildCard.Left);

            Assert.AreEqual(1, employees.Count());
        }

        [TestMethod]
        public void Session_GetAll_OrderedByFirstName()
        {
            var employee = new Session()
                .GetAll<Employee>().OrderBy(e => e.FirstName)
                .First();

            EntitiesAsserts.Assert_Employee_2(employee);

        }

        [TestMethod]
        public void Session_GetEmployeeList_ReportsToFilter()
        {
            /* 
             * Select * from Employees Where ReportsTo in 
             *      (Select EmployeeId From Employees Where FirstName = 'Andrew')
             */
            var employees1 = new Session()
                .GetAll<Employee>().Where(e => e.ReportsTo.FirstName == "Andrew");

            EntitiesAsserts.Assert_Employee_1(employees1.First());

            /* 
             * Select * From Employees Where ReportsTo in 
             *      (Select EmployeeId From Employees Where EmployeeId = 1)
             */
            var employees2 = new Session()
                .GetAll<Employee>().Where(e => e.ReportsTo.EmployeeId == 1);

            EntitiesAsserts.Assert_Employee_2(employees2.First());
        }

        [TestMethod]
        public void Session_GetAll_OrderedByFirstName_TSql()
        {
            var employee = new Session()
                .GetAll<Employee>().OrderBy(e => e.FirstName)
                .First();

            EntitiesAsserts.Assert_Employee_2(employee);

        }

        [TestMethod]
        public void Session_GetAll_OrderedByCity_Descending()
        {
            var employee2 = new Session()
                .GetAll<Employee>()
                .OrderBy(e => e.City)
                .Descending()
                .First();

            EntitiesAsserts.Assert_Employee_2(employee2);

            var employee3 = new Session()
                .GetAll<Employee>()
                .OrderBy(e => e.City)
                .First();

            EntitiesAsserts.Assert_Employee_3(employee3);

        }


        [TestMethod]
        public void QueryWith_OrCondition()
        {
            var employees = new Session()
                 .GetAll<Employee>()
                      .Where(m => m.EmployeeId == 1)
                      .Or(m => m.EmployeeId == 4)
                      .Or(m => m.EmployeeId == 2)
                      .Or(m=> m.EmployeeId == 5)
                      .And(m => m.EmployeeId > 3);

            Assert.AreEqual(2, employees.Count());
        }

        [TestMethod]
        public void MoreComplexQuery2()
        {
            var employees = new Session()
                 .GetAll<Employee>()
                      .Where(e => e.LastName).Like("r")
                      .Or(e => e.LastName).Like("a")
                 .OrderBy(m => m.City)
                 .Descending();

            Assert.AreEqual(8, employees.Count());
        }

        [TestMethod]
        public void MoreComplexQuery()
        {
            var employees = new Session()
                 .GetAll<Employee>()
                      .Where(m => m.EmployeeId > 0)
                      .And(m => m.EmployeeId < 10)
                 .OrderBy(m => m.City)
                 .Descending();

            Assert.AreEqual(9, employees.Count());
        }

        [TestMethod]
        public void Session_GetAll_withConditionExpression()
        {
            var id = 1;
            var employees = new Session()
                .GetAll<Employee>().Where(m => m.EmployeeId == id);

            Assert.AreEqual(1, employees.Count());
        }

        [TestMethod]
        public void Session_GetAll_withInConditionExpression()
        {
            var employees = new Session()
                .GetAll<Employee>()
                    .Where(m => m.EmployeeId)
                    .In(new[] { 1, 2, 3 });

            Assert.AreEqual(3, employees.Count());
        }

        [TestMethod]
        public void Session_GetAll_withInConditionExpression_Complex()
        {
            var employees = new Session()
                .GetAll<Employee>()
                    .Where(m => m.EmployeeId).In(new[] { 1, 2, 3, 4, 5, 6, 7 })
                    .And(m => m.EmployeeId >= 2);

            Assert.AreEqual(6, employees.Count());
        }

        [TestMethod]
        public void Session_GetAll_Between()
        {
            var employees = new Session()
                .GetAll<Employee>()
                    .Where(m => m.EmployeeId)
                    .Between(1, 5);

            Assert.AreEqual(5, employees.Count());
        }

        [TestMethod]
        public void Session_GetAll_In_using_collection()
        {
            ISession session = new Session();
            var collection = session.GetAll<Employee>()
                .Where(m => m.EmployeeId <= 5);

            var employees = session.GetAll<Employee>()
                .Where(m => m.EmployeeId)
                .In(collection);

            Assert.AreEqual(5, employees.Count());
        }

        //[TestMethod]
        //public void Session_GetAll_In_using_collection_SameNames()
        //{
        //    ISession session = new Session();

        //    var employee = session.GetById(new Employee { EmployeeId = 5 });

        //    var employees = session.GetAll<Employee>()
        //        .Where(m => m.EmployeeId)
        //        .In(employee.ReportedFrom);

        //    Assert.AreEqual(3, employees.Count());
        //}

        //[TestMethod]
        //public void Session_GetAll_In_using_collection_diffNames()
        //{
        //    ISession session = new Session();

        //    var orders = session.GetAll<Orders>()
        //        .Where(o => o.Shipper)
        //        .In(session.GetAll<Shippers>().Where(s => s.ShipperId == 2))
        //        .And(o => o.Employee.EmployeeId == 4);

        //    Assert.AreEqual(70, orders.Count());
        //}

        [TestMethod]
        public void Session_GetAll_withConditionExpression_IdConst()
        {
            var employees = new Session()
                .GetAll<Employee>().Where(m => m.EmployeeId == 1);

            Assert.AreEqual(1, employees.Count());
        }

        [TestMethod]
        public void Session_Count()
        {
            Assert.AreEqual(9, new Session().Count<Employee>());
        }

        [TestMethod]
        public void Session_GetAll_withWhereCond()
        {
            var employees = new Session()
                .GetAll<Employee>()
                .Where("EmployeeId = {0}", 1);

            Assert.AreEqual(1, employees.Count());
        }

        [TestMethod]
        public void Session_GetAll_withWhereCond_AndMoreConditions()
        {
            var employees = new Session()
                .GetAll<Employee>()
                .Where("FirstName = '{0}'", "Nancy")
                .And("LastName = '{0}'", "Davolio");

            Assert.AreEqual(1, employees.Count());
        }

        [TestMethod]
        public void Session_GetById()
        {
            var employee = new Session().GetById(new Employee { EmployeeId = 1 });

            EntitiesAsserts.Assert_Employee_1(employee);
        }

        [TestMethod]
        public void Session_Exists()
        {
            using (var session = new Session())
            {
                Assert.IsTrue(session.Exists(new Employee { EmployeeId = 1 }));
                Assert.IsTrue(session.Exists(new Employee { EmployeeId = 2 }));
                Assert.IsTrue(session.Exists(new Employee { EmployeeId = 3 }));

                Assert.IsFalse(session.Exists(new Employee { EmployeeId = 123123 }));
            }
        }

        [TestMethod]
        public void Session_GetById_Territories()
        {
            var employee = new Session().GetById(
                new Employee { EmployeeId = 1 });

            Assert.AreEqual(1, employee.Territories.Count());

            EntitiesAsserts.Assert_Employee_1(employee);
        }

        [TestMethod]
        public void Session_GetById_ReportsTo()
        {
            var employee = new Session().GetById(
                new Employee { EmployeeId = 1 });

            EntitiesAsserts.Assert_Employee_2(employee.ReportsTo);
            EntitiesAsserts.Assert_Employee_1(employee.ReportsTo.ReportsTo);
            EntitiesAsserts.Assert_Employee_2(employee.ReportsTo.ReportsTo.ReportsTo);
            EntitiesAsserts.Assert_Employee_1(employee);
        }


        [TestMethod]
        public void Session_Insert_Employee()
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
        public void Session_Delete_Employee()
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
        public void Session_Update_Employee()
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
