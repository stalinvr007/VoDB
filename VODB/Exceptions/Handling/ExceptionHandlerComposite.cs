using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VODB.Exceptions.Handling
{
    class ExceptionHandlerComposite : IExceptionHandler, IExceptionHandlerCollection
    {
        private readonly ICollection<IExceptionHandler> handlers = new List<IExceptionHandler>();

        public void RegisterExceptionHandler(IExceptionHandler handler)
        {
            handlers.Add(handler);
        }

        public void UnRegisterExceptionHandler(IExceptionHandler handler)
        {
            handlers.Remove(handler);
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
