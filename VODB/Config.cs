using System;
using System.Linq;
using System.Reflection;
using VODB.Core;
using VODB.Exceptions.Handling;

namespace VODB
{
    public static class Config
    {

        static IInternalExceptionHandlerCollection handlers = new ExceptionHandlerComposite();

        static Config()
        {
            handlers
                .RegisterExceptionHandler(new OnPrimaryKeyExceptionHandler())
                .RegisterExceptionHandler(new OnTruncatedExceptionHandler())
                .RegisterExceptionHandler(new OnUniqueKeyExceptionHandler());
        }

        internal static void MapNameSpace(Type type)
        {
            foreach (Type _type in Assembly.GetAssembly(type).GetTypes()
                .Where(t => !String.IsNullOrEmpty(t.Namespace))
                .Where(t => t.IsClass)
                .Where(t => t.Namespace.Equals(type.Namespace)))
            {
                Engine.Map(_type);
            }
        }

        #region EXCEPTION HANDLING

        public static IExceptionHandlerCollection RegisterExceptionHandler(IExceptionHandler handler)
        {
            handlers.RegisterExceptionHandler(handler);
            return handlers;
        }

        public static IExceptionHandlerCollection UnRegisterExceptionHandler(IExceptionHandler handler)
        {
            handlers.UnRegisterExceptionHandler(handler);
            return handlers;
        }

        internal static void UnRegisterAllExceptionHandlers()
        {
            handlers.UnregisterAllExceptionHandlers();   
        }

        #endregion
        
        #region PUBLIC ACCESS PROPERTIES

        /// <summary>
        /// Gets the Registered Exception Handlers.
        /// </summary>
        /// <value>
        /// The handlers.
        /// </value>
        public static IExceptionHandler Handlers
        {
            get
            {
                return handlers;
            }
        }
        
        #endregion

    }
}