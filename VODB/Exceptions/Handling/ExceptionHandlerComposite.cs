using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VODB.Exceptions.Handling
{
    class ExceptionHandlerComposite : IInternalExceptionHandlerCollection
    {
        private readonly ICollection<IExceptionHandler> handlers = new List<IExceptionHandler>();

        public IExceptionHandlerCollection RegisterExceptionHandler(IExceptionHandler handler)
        {
            handlers.Add(handler);
            return this;
        }

        public IExceptionHandlerCollection UnRegisterExceptionHandler(IExceptionHandler handler)
        {
            handlers.Remove(handler);
            return this;
        }

        public void UnregisterAllExceptionHandlers()
        {
            handlers.Clear();
        }

        public bool CanHandle(Exception exception)
        {
            return handlers.FirstOrDefault(f => f.CanHandle(exception)) != null;
        }

        public void Handle(Exception exception)
        {
            foreach (var handler in handlers.Where(f => f.CanHandle(exception)))
            {
                handler.Handle(exception);
            }
        }
    }
}
