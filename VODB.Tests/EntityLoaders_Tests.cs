﻿using System.Data.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VODB.DbLayer;
using VODB.Core.Loaders;
using VODB.Tests.Models.Northwind;

namespace VODB.Tests
{
    [TestClass]
    public class EntityLoaders_Tests
    {
        [TestMethod]
        public void EntityKeyLoader_Employees()
        {
            using (DbConnection con = new NameConventionDbConnectionCreator("System.Data.SqlClient").Create())
            {
                con.Open();

                var employee = new Employee();

                var cmd = con.CreateCommand();
                cmd.CommandText = Utils.EmployeeTable.CommandsHolder.Select;

                var reader = cmd.ExecuteReader();

                Assert.IsNotNull(reader);
                Assert.IsTrue(reader.Read());

                new EntityKeyLoader(new CachedEntities()).Load(employee, null, reader);

                reader.Close();

                Assert.AreEqual(1, employee.EmployeeId);
                Assert.IsNull(employee.FirstName);
            }
        }


        [TestMethod]
        public void FullEntityLoader_Employees()
        {
            using (var con = new NameConventionDbConnectionCreator("System.Data.SqlClient").Create())
            {
                con.Open();

                var employee = new Employee();

                var cmd = con.CreateCommand();
                cmd.CommandText = Utils.EmployeeTable.CommandsHolder.Select;

                var reader = cmd.ExecuteReader();

                Assert.IsNotNull(reader);
                Assert.IsTrue(reader.Read());

                new FullEntityLoader(new CachedEntities()).Load(employee, null, reader);

                reader.Close();

                EntitiesAsserts.Assert_Employee_1(employee);
            }
        }

        [TestMethod]
        public void ForeignKey_Setter()
        {

            Utils.Execute(session =>
            {
                var entity1 = session.GetById(new Employee { EmployeeId = 1 });
                var entity2 = session.GetById(new Employee { EmployeeId = 2 });

                entity1.ReportsTo = entity2.ReportsTo;

                Assert.AreEqual(entity1.ReportsTo.EmployeeId, entity2.ReportsTo.EmployeeId);
                Assert.ReferenceEquals(entity1.ReportsTo, entity2.ReportsTo);
            });

        }

    }
}