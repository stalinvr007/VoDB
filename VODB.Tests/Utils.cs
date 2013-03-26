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

        public static IDbConnectionCreator ConnectionCreator
        {
            get { return new NameConventionDbConnectionCreator("System.Data.SqlClient"); }
        }

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

        public static void ExecuteWith(Action<DbConnection, DbTransaction> action)
        {
            ExecuteWith(con =>
            {
                var trans = con.BeginTransaction();
                try
                {
                    action(con, trans);
                }
                finally
                {
                    trans.Rollback();
                }
            });
        }

        static Dictionary<Type, int> recordCounts = new Dictionary<Type, int>() {
            { typeof(Categories), 8 },
            { typeof(CustomerCustomerDemo), 0 },
            { typeof(CustomerDemographics), 0 },
            { typeof(Customers), 91 },
            { typeof(Employee), 9 },
            { typeof(EmployeeTerritories), 49 },
            { typeof(OrderDetails), 2155 },
            { typeof(Orders), 830 },
            { typeof(Products), 77 },
            { typeof(Region), 4 },
            { typeof(Shippers), 3 },
            { typeof(Suppliers), 29 },
            { typeof(Territories), 53 }
        };

        public static IDictionary<Type, int> RecordCounts
        {
            get { return recordCounts; }
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

        static IEntityTranslator translator = new CachingTranslator(new EntityTranslator());

        public static IEnumerable<ITable> ToTables(this IEnumerable<Type> type)
        {
            return TestModels.ToTables(translator);
        }

    }
}