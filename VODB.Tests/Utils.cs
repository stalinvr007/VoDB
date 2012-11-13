using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VODB.Tests
{
    public static class Utils
    {

        public static void Execute(Action<Session> action)
        {
            var session = new Session();

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
