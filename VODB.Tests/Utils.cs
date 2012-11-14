using System;

namespace VODB.Tests
{
    public static class Utils
    {
        public static void EagerExecute(Action<Session> action)
        {
            var session = new EagerSession();

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

        public static void EagerExecuteWithinTransaction(Action<Session> action)
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


        public static void StayAliveEagerExecute(Action<Session> action)
        {
            var session = new StayAliveEagerSession();

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

        public static void StayAliveEagerExecuteWithinTransaction(Action<Session> action)
        {
            StayAliveEagerExecute((session) =>
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