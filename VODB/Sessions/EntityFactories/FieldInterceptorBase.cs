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

        private readonly IDictionary<MethodInfo, PropertyValue> Properties = new Dictionary<MethodInfo, PropertyValue>();

        protected FieldInterceptorBase(Boolean interceptCollections)
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
                // Handle the setter to set the value for this property.
                HandleSetter(method, invocation);
            }
            else if (method.Name.StartsWith("get_"))
            {
                // Handle the getter to return the property value.
                HandleGetter(method, invocation);
            }
        }

        private void HandleSetter(MethodInfo method, IInvocation invocation)
        {
            // Find the get method
            var getter = invocation.TargetType.GetMethod("g" + method.Name.Remove(0, 1));

            PropertyValue propValue;
            if (Properties.TryGetValue(getter, out propValue))
            {
                // Resets the value...
                propValue.Value = invocation.ReturnValue;
                return;
            }

            // Sets the value and caches it.
            Properties[getter] = new PropertyValue
            {
                Value = invocation.ReturnValue
            };

        }

        private void HandleGetter(MethodInfo method, IInvocation invocation)
        {
            PropertyValue propValue;
            // Try to fetch from cache and check if this is fully loaded.
            if (Properties.TryGetValue(method, out propValue) && propValue.IsLoaded)
            {
                invocation.ReturnValue = propValue.Value;
                return;
            }

            // 
            var value = GetFieldValue(method, invocation);

            if (propValue == null)
            {
                // Cache the returned value from the HandleFieldGetValue;
                propValue = Properties[method] = new PropertyValue
                {
                    // If this is a collection field interceptor then the value 
                    // returned should allways be considered as loaded.
                    IsLoaded = InterceptCollections 
                };
            }

            // Cache and return the value.
            propValue.Value = invocation.ReturnValue = value;
        }

        /// <summary>
        /// Gets the field value.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="invocation">The invocation.</param>
        /// <returns></returns>
        protected abstract object GetFieldValue(MethodInfo method, IInvocation invocation);

    }
}
