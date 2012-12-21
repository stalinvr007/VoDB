using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace VODB.Core.Loaders.Factories
{
    public class Interceptor : IInterceptor
    {
        private readonly ISession _Session;

        IDictionary<MethodInfo, Object> lastResult = new Dictionary<MethodInfo, Object>();
        
        public Interceptor(ISession session)
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

            invocation.ReturnValue = lastResult[method] = _Session.GetById(invocation.ReturnValue);
        }

    }

    internal class EntityProxyFactory : IEntityFactory
    {

        static ProxyGenerator proxyGenerator = new ProxyGenerator();

        public Object Make(Type type, ISession session)
        {
            if (type.Namespace.Equals("Castle.Proxies"))
            {
                return Make(type.BaseType, session);
            }
            
            var result = proxyGenerator.CreateClassProxy(type, new Interceptor(session));

            return result;
        }
    }

    public interface IEntityFactory
    {
        Object Make(Type type, ISession session);
    }
}
