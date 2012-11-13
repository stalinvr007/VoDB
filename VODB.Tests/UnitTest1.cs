using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VODB.Tests.Models.Northwind;
using System.Linq;

namespace VODB.Tests
{
    [TestClass]
    public class Sessions_Tests
    {
        [TestMethod]
        public void EagerSession_GetAll()
        {

            var employees = Sessions.GetEager().GetAll<Employee>();

            Assert.AreEqual(9, employees.Count());

        }
    }
}
