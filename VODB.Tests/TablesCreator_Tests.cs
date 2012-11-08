using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VODB.VirtualDataBase;
using System.Threading;
using System.Linq;
using VODB.Caching;

namespace VODB.Tests
{
    class Entity
    {
        public String Name { get; set; }

        public int Age { get; set; }
    }

    [TestClass]
    public class TablesCreator_Tests
    {
        [TestMethod]
        public void CreateTable_Test()
        {
            var tableCreator = new TableCreator<Entity>();

            var table = tableCreator.Create();

            Assert.IsNotNull(table.KeyFields);
            Assert.IsNotNull(table.Fields);

            Assert.AreEqual(2, table.Fields.Count());
        }

        [TestMethod]
        public void CreateTable_FromTablesCache_Test()
        {
            TablesCache.AsyncAdd<Entity>(new TableCreator<Entity>());
            
            Table table;
            while ((table = TablesCache.GetTable<Entity>()) == null)
            {
                Thread.Yield();
            }

            Assert.IsNotNull(table.KeyFields);
            Assert.IsNotNull(table.Fields);

            Assert.AreEqual(2, table.Fields.Count());
        }
    }

}
