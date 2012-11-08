using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VODB.VirtualDataBase;
using System.Threading;
using System.Linq;
using VODB.Caching;
using VODB.Annotations;

namespace VODB.Tests
{

    [DbTable("MyTable")]
    class AnnotationsEntity : DbEntity
    {
        [DbIdentity]
        public String Id { get; set; }

        [DbKey]
        public String Name { get; set; }

        [DbField("Age1")]
        public int Age { get; set; }

    }

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


        [TestMethod]
        public void CreateTable_UsingAnnotations_Test()
        {
            var entity = new AnnotationsEntity();

            Assert.IsNotNull(entity.Table.KeyFields);
            Assert.IsNotNull(entity.Table.Fields);

            var fields = entity.Table.Fields.ToList();

            Assert.AreEqual("Id", fields[0].FieldName);
            Assert.AreEqual("Name", fields[1].FieldName);
            Assert.AreEqual("Age1", fields[2].FieldName);

            Assert.AreEqual(typeof(String), fields[0].FieldType);
            Assert.AreEqual(typeof(String), fields[1].FieldType);
            Assert.AreEqual(typeof(int), fields[2].FieldType);


            Assert.IsTrue(fields[0].IsKey);
            Assert.IsTrue(fields[1].IsKey);
            Assert.IsFalse(fields[2].IsKey);
        }

    }

}
