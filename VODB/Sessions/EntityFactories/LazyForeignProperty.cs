using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace VODB.Sessions.EntityFactories
{
    internal class LazyForeignProperty : IFieldInterceptor
    {
        private IInternalSession _Session;

        public LazyForeignProperty(IInternalSession session)
        {
            _Session = session;
        }

        public void Intercept(IInvocation invocation)
        {
            invocation.Proceed();
            MethodInfo method = invocation.Method;

            invocation.ReturnValue = _Session.GetById(invocation.ReturnValue);
        }

        public bool InterceptCollections
        {
            get { return false; }
        }
    }
}
