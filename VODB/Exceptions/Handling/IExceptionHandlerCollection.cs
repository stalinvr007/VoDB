using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VODB.Exceptions.Handling
{
    public interface IExceptionHandlerCollection
    {
        /// <summary>
        /// Adds the specified handler.
        /// </summary>
        /// <param name="handler">The handler.</param>
        void RegisterExceptionHandler(IExceptionHandler handler);
        /// <summary>
        /// Removes the specified handler.
        /// </summary>
        /// <param name="handler">The handler.</param>
        void UnRegisterExceptionHandler(IExceptionHandler handler);
    }
}
