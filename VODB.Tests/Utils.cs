using System;
using System.Threading;
using VODB.Core;
using VODB.Core.Infrastructure;
using VODB.Tests.Models.Northwind;

namespace VODB.Tests
{
    public static class Utils
    {

        static Utils()
        {
            Config.Map<ExpressionParser_Tests.Model>();
            Config.Map("VODB.Tests.Models.Northwind");            
        }

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


    }
}