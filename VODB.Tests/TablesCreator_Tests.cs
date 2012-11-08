using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VODB.VirtualDataBase;
using System.Threading;
using System.Linq;
using VODB.Caching;

namespace VODB.Tests
{

    class AutoCachedEntity : DbEntity
    {
        public String Name { get; set; }

        public int Age { get; set; }
    }

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
            var tableCreator = new TableCreator(typeof(Entity));

            var table = tableCreator.Create();

            Assert.IsNotNull(table.KeyFields);
            Assert.IsNotNull(table.Fields);

            Assert.AreEqual(2, table.Fields.Count());
        }

        [TestMethod]
        public void CreateTable_FromTablesCache_Test()
        {
            TablesCache.AsyncAdd<Entity>(new TableCreator(typeof(Entity)));
            
            Table table;
            while ((table = TablesCache.GetTable<Entity>()) == null)
            {
                Thread.Yield();
            }

            Assert.IsNotNull(table.KeyFields);
            Assert.IsNotNull(table.Fields);

            Assert.AreEqual(2, table.Fields.Count());
        }

        [TestMethod]
        public void CreateTable_FromDbEntity_Test()
        {
            
            var entity = new AutoCachedEntity();
            
            Table table;
            while ((table = TablesCache.GetTable<AutoCachedEntity>()) == null)
            {
                Thread.Yield();
            }

            Assert.IsNotNull(table.KeyFields);
            Assert.IsNotNull(table.Fields);

            Assert.AreEqual(2, table.Fields.Count());
        }


        [TestMethod]
        public void CreateTable_FromDbEntity_MultipleInstances_Test()
        {

            for (int i = 0; i < 1000; i++)
            {
                /* Makes a call to AsyncAdd for each instance. */
                new AutoCachedEntity();
            }

            Assert.AreEqual(2, TablesCache.GetTables().Count());

        }

    }

}
