using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VODB.Exceptions.Handling
{

    internal interface IInternalExceptionHandlerCollection : IExceptionHandlerCollection, IExceptionHandler
    {

        /// <summary>
        /// Unregisters all exception handlers.
        /// </summary>
        void UnregisterAllExceptionHandlers();
    }

    public interface IExceptionHandlerCollection
    {
        /// <summary>
        /// Adds the specified handler.
        /// </summary>
        /// <param name="handler">The handler.</param>
        IExceptionHandlerCollection RegisterExceptionHandler(IExceptionHandler handler);
        /// <summary>
        /// Removes the specified handler.
        /// </summary>
        /// <param name="handler">The handler.</param>
        IExceptionHandlerCollection UnRegisterExceptionHandler(IExceptionHandler handler);
    }
}
