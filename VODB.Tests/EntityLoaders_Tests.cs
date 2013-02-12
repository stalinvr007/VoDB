using System.Data.Common;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using VODB.DbLayer;
using VODB.Core.Loaders;
using VODB.Tests.Models.Northwind;
using System.Diagnostics;
using System.Collections.Generic;
using System;
using VODB.Core.Loaders.Factories;
using VODB.Core;

namespace VODB.Tests
{
    [TestFixture]
    public class EntityLoaders_Tests
    {
        [Test]
        public void EntityKeyLoader_Employees()
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

                new EntityKeyLoader(new CachedEntities()).Load(employee, null, reader);

                reader.Close();

                Assert.AreEqual(1, employee.EmployeeId);
                Assert.IsNull(employee.FirstName);
            }
        }


        [Test]
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

        [Test]
        public void ForeignKey_Setter()
        {

            Utils.Execute(session =>
            {
                var entity1 = session.GetById(new Employee { EmployeeId = 1 });
                var entity2 = session.GetById(new Employee { EmployeeId = 2 });

                entity1.ReportsTo = entity2.ReportsTo;

                Assert.AreEqual(entity1.ReportsTo.EmployeeId, entity2.ReportsTo.EmployeeId);
            });

        }

        [Test]
        public void DbReaderMapper()
        {
            Utils.ExecuteWith(con =>
            {
                var cmd = con.CreateCommand();
                cmd.CommandText = Utils.EmployeeTable.CommandsHolder.Select;

                var reader = cmd.ExecuteReader();
                try
                {
                    var session = Engine.Get<ISession>() as IInternalSession;
                    var task = new DbReaderMapper(session, new DictionaryMapper(session), new EntityProxyFactory())
                     .Map<Employee>(reader);

                    Assert.AreEqual(9, task.Result.Count());

                    var employees = task.Result;

                    EntitiesAsserts.Assert_Employee_1(employees.First());
                }
                catch (AggregateException)
                {
                    throw;
                }
            });


        }
        
        [Test]
        public void DbReaderMapper_FullEntityLoader_Comparation()
        {
            Utils.ExecuteWith(con =>
            {
                var cmd = con.CreateCommand();
                cmd.CommandText = Utils.EmployeeTable.CommandsHolder.Select;

                var reader = cmd.ExecuteReader();
                var entityFactory = new EntityProxyFactory();

                var session = Engine.Get<ISession>() as IInternalSession;
                entityFactory.Make<Employee>(session);

                var whatch = new Stopwatch();
                whatch.Start();
                
                var task = new DbReaderMapper(session, new DictionaryMapper(session), entityFactory)
                     .Map<Employee>(reader);

                task.Wait();
                whatch.Stop();
                
                reader = cmd.ExecuteReader();
                var whatch1 = new Stopwatch();
                whatch1.Start();
                var fullEntityLoader = new FullEntityLoader(new CachedEntities());

                var result = new List<Employee>();
                while (reader.Read())
                {
                    var employee = entityFactory.Make<Employee>(session);
                    fullEntityLoader.Load(employee, session, reader);
                    result.Add(employee);
                }
                reader.Close();

                whatch1.Stop();

                Assert.AreEqual(9, result.Count);
                Assert.AreEqual(9, task.Result.Count());
                Assert.Less(whatch.ElapsedTicks, whatch1.ElapsedTicks);

                Console.WriteLine("DbReaderMapper     : {0,5} ticks", whatch.ElapsedTicks);
                Console.WriteLine("FullEntityLoader   : {0,5} ticks", whatch1.ElapsedTicks);
            });


        }
    }
}