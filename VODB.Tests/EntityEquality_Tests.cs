using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VODB.Tests.Models.Northwind;

namespace VODB.Tests
{
    [TestClass]
    public class EntityEquality_Tests
    {
        [TestMethod]
        public void EqualityTest_Employee()
        {
            var employee1 = new Employee { EmployeeId = 1 };
            var employee2 = new Employee { EmployeeId = 2 };

            Assert.IsFalse(new EntityEquality<Employee>().Equals(employee1, employee2));
        }

        [TestMethod]
        public void EqualityTest_Employee_Equals()
        {
            var employee1 = new Employee { EmployeeId = 1 };
            var employee2 = new Employee { EmployeeId = 1 };

            Assert.IsTrue(new EntityEquality<Employee>().Equals(employee1, employee2));
        }

    }
}
