using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VODB.Annotations;
using VODB.Caching;
using VODB.VirtualDataBase;

namespace VODB.Tests
{
    [DbTable("MyTable")]
    internal class AnnotationsEntity : DbEntity
    {
        [DbIdentity]
        public String Id { get; set; }

        [DbKey]
        public String Name { get; set; }

        [DbField("Age1"), DbRequired]
        public int Age { get; set; }
    }

    internal class AutoCachedEntity : DbEntity
    {
        [DbKey]
        public String Name { get; set; }

        public int Age { get; set; }
    }

    internal class Entity
    {
        public String Name { get; set; }

        public int Age { get; set; }
    }

    [TestClass]
    public class TablesCreator_Tests
    {
        [TestMethod, ExpectedException(typeof (AggregateException))]
        public void CreateTable_Test()
        {
            var tableCreator = new TableCreator(typeof (Entity));

            Table table = tableCreator.Create();

            Assert.IsNotNull(table.KeyFields);
            Assert.IsNotNull(table.Fields);

            Assert.IsNotNull(table.CommandsHolder.Update);

            Assert.AreEqual(2, table.Fields.Count());
        }

        [TestMethod]
        public void CreateTable_FromTablesCache_Test()
        {
            TablesCache.AsyncAdd<Entity>(new TableCreator(typeof (Entity)));

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
            new AutoCachedEntity();

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

            Assert.IsTrue(
                TablesCache.GetTables().Count(t => t.TableName.Equals("AutoCachedEntity")) == 1
                );
        }


        [TestMethod]
        public void CreateTable_UsingAnnotations_Test()
        {
            var entity = new AnnotationsEntity();

            Assert.IsNotNull(entity.Table.KeyFields);
            Assert.IsNotNull(entity.Table.Fields);

            List<Field> fields = entity.Table.Fields.ToList();

            Assert.AreEqual("MyTable", entity.Table.TableName);

            Assert.AreEqual("Id", fields[0].FieldName);
            Assert.AreEqual("Name", fields[1].FieldName);
            Assert.AreEqual("Age1", fields[2].FieldName);

            Assert.IsFalse(fields[0].IsRequired);
            Assert.IsFalse(fields[1].IsRequired);
            Assert.IsTrue(fields[2].IsRequired);

            Assert.AreEqual(typeof (String), fields[0].FieldType);
            Assert.AreEqual(typeof (String), fields[1].FieldType);
            Assert.AreEqual(typeof (int), fields[2].FieldType);


            Assert.IsTrue(fields[0].IsKey);
            Assert.IsTrue(fields[1].IsKey);
            Assert.IsFalse(fields[2].IsKey);

            Assert.IsNotNull(entity.Table.CommandsHolder.Select);
        }
    }
}