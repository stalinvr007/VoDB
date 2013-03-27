using System;
using VODB.DbLayer;

namespace VODB.Tests
{
    public static class VodbConnectionExtensions
    {
        public static void WithRollback(this IVodbConnection connection, Action<IVodbConnection> action)
        {
            var transaction = connection.BeginTransaction();
            try
            {
                action(connection);
            }
            finally
            {
                transaction.Rollback();
            }
        }

        public static void WithRollback(this ISession connection, Action<ISession> action)
        {
            var transaction = connection.BeginTransaction();
            try
            {
                action(connection);
            }
            finally
            {
                transaction.Rollback();
            }
        }
    }
}
