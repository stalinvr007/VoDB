using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Fasterflect;

namespace VODB.Sessions.EntityFactories
{
    [Obsolete]
    class LazyForeignProperty : IFieldInterceptor
    {
        private IInternalSession _Session;

        private readonly IDictionary<MethodInfo, Object> lastResult = new Dictionary<MethodInfo, Object>();

        public LazyForeignProperty(IInternalSession session)
        {
            _Session = session;
        }

        public void Intercept(IInvocation invocation)
        {
            invocation.Proceed();


            MethodInfo method = invocation.Method;
            if (method.Name.StartsWith("set_"))
            {
                lastResult[method] = invocation.ReturnValue;
                return;
            }

            if (method.Name.StartsWith("get_"))
            {
                object result = null;
                if (lastResult.TryGetValue(method, out result))
                {
                    invocation.ReturnValue = result;
                    return;
                }

                lastResult[method] = invocation.ReturnValue = _Session.GetById(invocation.ReturnValue);
            }
        }

        public bool InterceptCollections
        {
            get { return false; }
        }
    }
}
