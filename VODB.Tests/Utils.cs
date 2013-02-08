using System;
using System.Data.Common;
using System.Threading;
using VODB.Core;
using VODB.Core.Infrastructure;
using VODB.DbLayer;
using VODB.Tests.Models.Northwind;

namespace VODB.Tests
{
    public static class Utils
    {

        internal static Table EmployeeTable
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


    }
}