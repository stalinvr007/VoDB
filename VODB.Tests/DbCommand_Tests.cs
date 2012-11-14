using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VODB.DbLayer;
using VODB.DbLayer.DbCommands;
using VODB.DbLayer.DbExecuters;
using VODB.DbLayer.Loaders;
using VODB.Tests.Models.Northwind;
using VODB.VirtualDataBase;

namespace VODB.Tests
{
    [TestClass]
    public class DbCommand_Tests
    {
        [TestMethod]
        public void GetEmployeesData()
        {
            using (DbConnection con = new DbConnectionCreator("System.Data.SqlClient").Create())
            {
                con.Open();

                Table table = new Employee().Table;
                DbCommand cmd = con.CreateCommand();
                cmd.CommandText = table.CommandsHolder.Select;

                var query = new DbQueryExecuterCommandEager(cmd, table);

                Assert.AreEqual(9, query.Execute().Count());

                con.Close();
            }
        }


        [TestMethod]
        public void GetEmployeesEntities()
        {
            Utils.Execute(session =>
                              {
                                  var factory = new DbEntitySelectCommandFactory<Employee>(session);

                                  var query = new DbEntityQueryExecuterEager<Employee>(factory,
                                                                                       new FullEntityLoader<Employee>());

                                  IEnumerable<Employee> result = query.Execute();

                                  Assert.AreEqual(9, result.Count());

                                  Assert.AreEqual(9, result.Count(emp => !String.IsNullOrEmpty(emp.FirstName)));
                              });
        }

        [TestMethod]
        public void GetEmployeesEntities_Lazy()
        {
            Utils.Execute(session =>
                              {
                                  var factory = new DbEntitySelectCommandFactory<Employee>(session);

                                  var query = new DbEntityQueryExecuterLazy<Employee>(factory,
                                                                                      new FullEntityLoader<Employee>());

                                  IEnumerable<Employee> result = query.Execute();

                                  Assert.AreEqual(9, result.Count(emp => !String.IsNullOrEmpty(emp.FirstName)));
                              });
        }

        [TestMethod]
        public void GetEmployeesById_Eager()
        {
            Utils.Execute(session =>
                              {
                                  var factory = new DbEntitySelectByIdCommandFactory<Employee>(session,
                                                                                               new Employee
                                                                                                   {EmployeeId = 1});

                                  var query = new DbEntityQueryExecuterEager<Employee>(factory,
                                                                                       new FullEntityLoader<Employee>());

                                  IEnumerable<Employee> result = query.Execute();

                                  Assert.AreEqual(1, result.Count());

                                  EntitiesAsserts.Assert_Employee_1(result.First());
                              });
        }

        [TestMethod]
        public void GetEmployeesById_Eager_NoIdSupplied()
        {
            Utils.Execute(session =>
                              {
                                  var factory = new DbEntitySelectByIdCommandFactory<Employee>(session,
                                                                                               new Employee());

                                  var query = new DbEntityQueryExecuterEager<Employee>(factory,
                                                                                       new FullEntityLoader<Employee>());

                                  IEnumerable<Employee> result = query.Execute();

                                  Assert.AreEqual(0, result.Count());
                              });
        }

        [TestMethod]
        public void InsertEmployee()
        {
            Utils.Execute(session =>
                              {
                                  var trans = session.BeginTransaction();

                                  try
                                  {
                                      var factory = new DbEntityInsertCommandFactory<Employee>(session,
                                                                                               new Employee());

                                      var query = new DbCommandNonQueryExecuter(factory);

                                      Assert.AreEqual(1, query.Execute());
                                  }
                                  finally
                                  {
                                      trans.RollBack();
                                  }
                              });
        }
    }
}