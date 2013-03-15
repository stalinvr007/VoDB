using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using VODB.Core;
using VODB.Core.Infrastructure;
using VODB.DbLayer;
using VODB.EntityTranslation;
using VODB.Infrastructure;
using VODB.Tests.Models.Northwind;

namespace VODB.Tests
{
    public static class Utils
    {

        internal static VODB.Core.Infrastructure.Table EmployeeTable
        {
            get
            {
                return Engine.GetTable<Employee>();
            }
        }

        public static void Execute(Action<ISession> action)
        {
            using (var session = Engine.Get<IInternalSession>())
            {
                session.Open();
                action(session);
            }
        }

        public static void ExecuteWithinTransaction(Action<ISession> action)
        {
            Execute(session =>
            {
                var trans = session.BeginTransaction();
                try
                {
                    action(session);
                }
                finally
                {
                    trans.RollBack();
                }
            });
        }

        public static void ExecuteWith(Action<DbConnection> action)
        {
            using (var con = new NameConventionDbConnectionCreator("System.Data.SqlClient").Create())
            {
                con.Open();
                try
                {
                    action(con);
                }
                finally
                {
                    con.Close();    
                }
            }
        }


        public static IEnumerable<Type> TestModels
        {
            get
            {
                yield return typeof(Categories);
                yield return typeof(CustomerCustomerDemo);
                yield return typeof(CustomerDemographics);
                yield return typeof(Customers);
                yield return typeof(Employee);
                yield return typeof(EmployeeTerritories);
                yield return typeof(OrderDetails);
                yield return typeof(Orders);
                yield return typeof(Products);
                yield return typeof(Region);
                yield return typeof(Shippers);
                yield return typeof(Suppliers);
                yield return typeof(Territories);
            }
        }

        public static IEnumerable<ITable> ToTables(this IEnumerable<Type> type, IEntityTranslator translator)
        {
            return TestModels
                .Select(t => translator.Translate(t));
        }

    }
}