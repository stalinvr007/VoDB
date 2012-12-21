using Castle.DynamicProxy;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using VODB.Core.Execution.Executers;
using VODB.Core.Execution.SqlPartialBuilders;

namespace VODB.Core.Loaders.Factories
{
    class Interceptor : IInterceptor
    {
        private readonly IInternalSession _Session;
        

        IDictionary<MethodInfo, Object> lastResult = new Dictionary<MethodInfo, Object>();

        public Interceptor(IInternalSession session)
        {
            _Session = session;
        }

        public void Intercept(IInvocation invocation)
        {
            invocation.Proceed();

            var method = invocation.Method;
            if (!method.Name.StartsWith("get_"))
            {
                return;
            }

            Object result;
            if (lastResult.TryGetValue(method, out result))
            {
                invocation.ReturnValue = result;
                return;
            }

            invocation.ReturnValue = lastResult[method] = ResolveResult(invocation);
        }

        private static IEnumerable<T> ProxyGenericIterator<T>(
            object target, IEnumerable enumerable)
        {
            return ProxyNonGenericIterator(target, enumerable).Cast<T>();
        }

        private static IEnumerable ProxyNonGenericIterator(
            object target, IEnumerable enumerable)
        {
            try
            {
                foreach (var element in enumerable)
                    yield return element;
            }
            finally
            { }
        }

        private static readonly MethodInfo ProxyGenericIteratorMethod =
            typeof(Interceptor)
                .GetMethod(
                    "ProxyGenericIterator",
                    BindingFlags.NonPublic | BindingFlags.Static);

        private object ResolveResult(IInvocation invocation)
        {
            if (invocation.Method.ReturnType.IsGenericType)
            {
                string fieldName = invocation.Method.Name.Remove(0, 4);
                var field = Engine.GetTable(invocation.Method.ReflectedType).CollectionFields.FindField(fieldName);
                var builder = new SqlQueryBuilder(field);


                Type entityType = invocation.Method.ReturnType.GetGenericArguments()[0];
                var method = ProxyGenericIteratorMethod.MakeGenericMethod(entityType);

                return method.Invoke(null, new[] { invocation.InvocationTarget,
                    Engine.Get<IQueryExecuter>().RunQuery(
                        entityType,
                        _Session,
                        builder.Query,
                        builder.Parameters) 
                });
            }
            else
            {
                return _Session.GetById(invocation.ReturnValue);
            }
        }

    }
}
