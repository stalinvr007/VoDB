using System;

namespace VODB
{

    public static class ISessionExtensions
    {

        /// <summary>
        /// Executes a functions Within a transaction.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="session">The session.</param>
        /// <param name="func">The func.</param>
        /// <returns></returns>
        public static TResult WithinTransaction<TResult>(this ISession session, Func<TResult> func)
        {

            var trans = session.BeginTransaction();
            try
            {
                return func();
            }
            finally
            {
                trans.Commit();
            }
            
        }


        /// <summary>
        /// Executes a action Within the transaction.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="action">The action.</param>
        public static void WithinTransaction(this ISession session, Action action)
        {
            session.WithinTransaction<Object>(() => { action(); return null; });
        }
    



    }



}
