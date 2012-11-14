using System;
using VODB.Sessions;

namespace VODB.Tests
{
    public static class Utils
    {
        public static void EagerExecute(Action<ISession> action)
        {
            var session = new EagerInternalSession();

            session.Open();

            try
            {
                action(session);
            }
            finally
            {
                session.Close();
            }
        }

        public static void EagerExecuteWithinTransaction(Action<ISession> action)
        {
            EagerExecute((session) =>
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