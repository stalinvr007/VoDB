using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VODB.Core.Loaders.Factories
{
    public class Interceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            invocation.Proceed();
        }
    }

    internal class EntityProxyFactory : IEntityFactory
    {

        private readonly IInterceptor _Interceptor;
        public EntityProxyFactory(IInterceptor interceptor)
        {
            _Interceptor = interceptor;
        }

        public Object Make(Type type)
        {
            var proxyGenerator = new ProxyGenerator();
            return proxyGenerator.CreateClassProxy(type, _Interceptor);
        }
    }

    public interface IEntityFactory
    {
        Object Make(Type type);
    }
}
