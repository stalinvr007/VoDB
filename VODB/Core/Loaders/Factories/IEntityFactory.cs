using System.Diagnostics;
using Castle.DynamicProxy;
using System;

namespace VODB.Core.Loaders.Factories
{
    internal class EntityProxyFactory : IEntityFactory
    {

        static readonly ProxyGenerator proxyGenerator = new ProxyGenerator();

        public Object Make(Type type, IInternalSession session)
        {
            Debug.Assert(type.Namespace != null, "type.Namespace != null");

            return type.Namespace.Equals("Castle.Proxies") ? 
                Make(type.BaseType, session) : 
                proxyGenerator.CreateClassProxy(type, new Interceptor(session));
        }
    }

    interface IEntityFactory
    {
        Object Make(Type type, IInternalSession session);
    }
}
