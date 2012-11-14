using System;

namespace VODB.Tests
{
    public static class Utils
    {
        public static void Execute(Action<Session> action)
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
    }
}