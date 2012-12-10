using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VODB.DbLayer.DbCommands;
using VODB.DbLayer.DbExecuters;
using VODB.DbLayer.Loaders;
using VODB.Tests.Models.Northwind;
using VODB.Exceptions;

namespace VODB.Tests
{
    [TestClass]
    public class DbCommand_Tests
    {

        [TestMethod]
        public void GetEmployeesEntities()
        {
            Utils.EagerExecute(session =>
            {
                var factory = new DbEntitySelectCommandFactory<Employee>(session as IInternalSession);

                var query = new DbEntityQueryExecuterEager<Employee>((IInternalSession)session,
                    factory,
                    new FullEntityLoader<Employee>());

                IEnumerable<Employee> result = query.Execute();

                Assert.AreEqual(9, result.Count());

                Assert.AreEqual(9, result.Count(emp => !String.IsNullOrEmpty(emp.FirstName)));
            });
        }

        [TestMethod]
        public void GetEmployeesEntities_Lazy()
        {
            Utils.EagerExecute(session =>
            {
                var factory = new DbEntitySelectCommandFactory<Employee>(session as IInternalSession);

                var query = new DbEntityQueryExecuterLazy<Employee>(
                    (IInternalSession)session,
                    factory,
                    new FullEntityLoader<Employee>());

                IEnumerable<Employee> result = query.Execute();

                Assert.AreEqual(9, result.Count(emp => !String.IsNullOrEmpty(emp.FirstName)));
            });
        }

        [TestMethod]
        public void GetEmployeesById_Eager()
        {
            Utils.EagerExecute(session =>
            {
                var factory = new DbEntitySelectByIdCommandFactory<Employee>(
                    session as IInternalSession,
                    new Employee { EmployeeId = 1 });

                var query = new DbEntityQueryExecuterEager<Employee>((IInternalSession)session,
                    factory,
                    new FullEntityLoader<Employee>());

                IEnumerable<Employee> result = query.Execute();

                Assert.AreEqual(1, result.Count());

                EntitiesAsserts.Assert_Employee_1(result.First());
            });
        }

        [TestMethod, ExpectedException(typeof(ValidationException))]
        public void GetEmployeesById_Eager_NoIdSupplied()
        {
            Utils.EagerExecute(session =>
            {
                var factory = new DbEntitySelectByIdCommandFactory<Employee>(
                    session as IInternalSession,
                    new Employee());

                var query = new DbEntityQueryExecuterEager<Employee>(
                    (IInternalSession)session,
                    factory,
                    new FullEntityLoader<Employee>());

                query.Execute().Count();
            });
        }

        [TestMethod, ExpectedException(typeof(ValidationException))]
        public void InsertEmployee_NoFieldsSet()
        {
            Utils.EagerExecuteWithinTransaction(session =>
            {
                var cmdFactory = new DbEntityInsertCommandFactory<Employee>(session as IInternalSession, new Employee());
                var query = new DbCommandNonQueryExecuter(cmdFactory);

                query.Execute();
            });
        }

        [TestMethod]
        public void InsertEmployee()
        {
            Utils.EagerExecuteWithinTransaction(
                session =>
                {
                    var employee = new Employee
                    {
                        EmployeeId = 10,
                        FirstName = "Sérgio",
                        LastName = "Ferreira"
                    };

                    var cmdFactory = new DbEntityInsertCommandFactory<Employee>(session as IInternalSession, employee);
                    var insert = new DbCommandNonQueryExecuter(cmdFactory);

                    var count = session.GetAll<Employee>().Count();
                    insert.Execute();


                    Assert.IsTrue(employee.EmployeeId > 0);
                    Assert.AreEqual(count + 1, session.GetAll<Employee>().Count());
                });
        }
    }
}