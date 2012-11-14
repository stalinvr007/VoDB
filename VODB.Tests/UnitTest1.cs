using System.Collections.Generic;
using System.Linq;
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
            IEnumerable<Employee> employees = Sessions.GetEager().GetAll<Employee>();

            Assert.AreEqual(9, employees.Count());
        }
    }
}