using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VODB.Annotations;
using VODB.Caching;
using VODB.Tests.Models.Northwind;
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

            Assert.AreEqual(1, TablesCache.GetTables().Count(t => t.TableName.Equals("AutoCachedEntity")));
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
            Assert.IsTrue(fields[1].IsRequired);
            Assert.IsTrue(fields[2].IsRequired);

            Assert.AreEqual(typeof (String), fields[0].FieldType);
            Assert.AreEqual(typeof (String), fields[1].FieldType);
            Assert.AreEqual(typeof (int), fields[2].FieldType);


            Assert.IsTrue(fields[0].IsKey);
            Assert.IsTrue(fields[1].IsKey);
            Assert.IsFalse(fields[2].IsKey);

            Assert.IsNotNull(entity.Table.CommandsHolder.Select);
        }

        [TestMethod]
        public void CreateTable_Employee_Test()
        {
            var entity = new Employee();

            Assert.IsNotNull(entity.Table.KeyFields);
            Assert.IsNotNull(entity.Table.Fields);

            List<Field> fields = entity.Table.Fields.ToList();

            Assert.AreEqual("Employees", entity.Table.TableName);

            Assert.AreEqual("EmployeeId", fields[0].FieldName);
            Assert.AreEqual("LastName", fields[1].FieldName);
            Assert.AreEqual("FirstName", fields[2].FieldName);
            Assert.AreEqual("Title", fields[3].FieldName);
            Assert.AreEqual("TitleOfCourtesy", fields[4].FieldName);
            Assert.AreEqual("BirthDate", fields[5].FieldName);
            Assert.AreEqual("HireDate", fields[6].FieldName);
            Assert.AreEqual("Address", fields[7].FieldName);
            Assert.AreEqual("City", fields[8].FieldName);
            Assert.AreEqual("Region", fields[9].FieldName);
            Assert.AreEqual("PostalCode", fields[10].FieldName);
            Assert.AreEqual("Country", fields[11].FieldName);
            Assert.AreEqual("HomePhone", fields[12].FieldName);
            Assert.AreEqual("Extension", fields[13].FieldName);
            Assert.AreEqual("Notes", fields[14].FieldName);
            Assert.AreEqual("Photo", fields[15].FieldName);
            Assert.AreEqual("ReportsTo", fields[16].FieldName);
            Assert.AreEqual("PhotoPath", fields[17].FieldName);

            Assert.IsFalse(fields[0].IsRequired);
            Assert.IsTrue(fields[1].IsRequired);
            Assert.IsTrue(fields[2].IsRequired);


            Assert.IsTrue(fields[0].IsKey);

            foreach (var field in fields.Skip(1))
            {
                Assert.IsFalse(field.IsKey);   
            }

            Assert.IsNotNull(entity.Table.CommandsHolder.Select);
        }
    }
}