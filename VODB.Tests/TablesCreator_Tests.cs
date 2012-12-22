using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VODB.Annotations;
using VODB.Tests.Models.Northwind;
using VODB.Core.Infrastructure;
using VODB.Core;

namespace VODB.Tests
{
    [DbTable("MyTable")]
    internal class AnnotationsEntity
    {
        [DbIdentity]
        public String Id { get; set; }

        [DbKey]
        public String Name { get; set; }

        [DbField("Age1"), DbRequired]
        public int Age { get; set; }
    }

    internal class AutoCachedEntity
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

        [TestMethod]
        public void CreateTable_Employee_Test()
        {
            var entity = new Employee();
            var table = Engine.GetTable<Employee>();

            Assert.IsNotNull(table.KeyFields);
            Assert.IsNotNull(table.Fields);

            List<Field> fields = table.Fields.ToList();

            Assert.AreEqual("Employees", table.TableName);

            AssertFields(fields);

            Assert.IsFalse(fields[0].IsRequired);
            Assert.IsTrue(fields[1].IsRequired);
            Assert.IsTrue(fields[2].IsRequired);


            Assert.IsTrue(fields[0].IsKey);

            foreach (var field in fields.Skip(1))
            {
                Assert.IsFalse(field.IsKey);   
            }

            Assert.IsNotNull(table.CommandsHolder.Select);
        }

        [TestMethod]
        public void FieldMapping_Test()
        {
            var fieldMapping = new FieldMapper<Employee>(new FieldMapper());
            var fields = fieldMapping.GetFields().ToList();

            AssertFields(fields);

        }

        [TestMethod]
        public void TableMapper_Test()
        {
            Engine.Map<Employee>();

            var table = Engine.GetTable<Employee>();
            
            Assert.IsNotNull(table);
            AssertFields(table.Fields.ToList());

            Assert.AreEqual("Employees", table.TableName);
        }

        private static void AssertFields(List<Field> fields)
        {
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

            Assert.IsTrue(fields[0].IsKey);

            Assert.AreEqual(fields.Count - 1, fields.Count(f => !f.IsKey));
        }
    
    }
}