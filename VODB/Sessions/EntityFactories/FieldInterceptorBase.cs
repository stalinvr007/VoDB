using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VODB.Sessions.EntityFactories
{
    internal abstract class FieldInterceptorBase : IFieldInterceptor
    {
        public FieldInterceptorBase(Boolean interceptCollections)
        {
            InterceptCollections = interceptCollections;
        }

        public Boolean InterceptCollections { get; private set; }

        public void Intercept(IInvocation invocation)
        {
            // Enable the intercepted method to execute.
            invocation.Proceed();

            var method = invocation.Method;

            if (method.Name.StartsWith("set_"))
            {
                HandleSetter(method);
            }
            else if (method.Name.StartsWith("get_"))
            {
                HandleGetter(method);
            }
        }

        /// <summary>
        /// Handles the setter.
        /// </summary>
        /// <param name="method">The method.</param>
        protected abstract void HandleSetter(MethodInfo method);

        /// <summary>
        /// Handles the getter.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <returns>The value that will be returned by the getter.</returns>
        protected abstract Object HandleGetter(MethodInfo method);

    }
}
