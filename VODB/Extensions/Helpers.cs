using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VODB.Extensions
{
    public static class Helpers
    {

        /// <summary>
        /// Throws ArgumentNullException if argument is null.
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="argumentName">Name of the argument.</param>
        /// <param name="msg">The MSG.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static void ThrowExceptionIfNull(this Object argument, String argumentName, String msg)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(argumentName, msg);
            }
        }

    }
}
