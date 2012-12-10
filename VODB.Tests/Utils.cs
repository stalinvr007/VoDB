using System;
using VODB.Sessions;

namespace VODB.Tests
{
    public static class Utils
    {
        public static void EagerExecute(Action<ISession> action)
        {
            using (var session = new InternalEagerSession())
            {
                session.Open();
                action(session);
            }
        }

        public static void EagerExecuteWithinTransaction(Action<ISession> action)
        {
            EagerExecute(session =>
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