using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Fasterflect;
using System.Collections;
using VODB.Core.Execution.Executers;

namespace VODB.Sessions.EntityFactories
{
    internal class LazyCollectionForeignProperty : IFieldInterceptor
    {
        private static readonly MethodInfo ProxyGenericIteratorMethod =
            typeof(LazyCollectionForeignProperty)
                .GetMethod(
                    "ProxyGenericIterator",
                    BindingFlags.NonPublic | BindingFlags.Static);

        private static readonly MethodInfo SessionGenericExecuteQueryMethod =
            typeof(ISession)
                .GetMethod(
                    "GetAll",
                    BindingFlags.Public | BindingFlags.Instance);

        private readonly IInternalSession _Session;
        private readonly IDictionary<MethodInfo, Object> lastResult = new Dictionary<MethodInfo, Object>();


        public LazyCollectionForeignProperty(IInternalSession session)
        {
            _Session = session;

        }

        public Boolean InterceptCollections { get { return true; } }

        private void SetResult(IInvocation invocation, MethodInfo method)
        {
            var entityType = method.ReturnParameter.ParameterType.GetGenericArguments()[0];

            MethodInfo methodIterator = ProxyGenericIteratorMethod.MakeGenericMethod(entityType);
            MethodInfo me = SessionGenericExecuteQueryMethod.MakeGenericMethod(entityType);

            object result = methodIterator.Invoke(null, new Object[] 
            {
                invocation.InvocationTarget,
                me.Invoke(_Session, new Object[]{ })
            });

            lastResult[method] = invocation.ReturnValue = result;
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

            object result = null;
            if (lastResult.TryGetValue(method, out result))
            {
                invocation.ReturnValue = result;
                return;
            }

            SetResult(invocation, method);
        }

        private static IEnumerable<T> ProxyGenericIterator<T>(object target, IEnumerable enumerable)
        {
            return ProxyNonGenericIterator(target, enumerable).Cast<T>();
        }

        private static IEnumerable ProxyNonGenericIterator(object target, IEnumerable enumerable)
        {
            return enumerable.Cast<object>();
        }
    }
}
