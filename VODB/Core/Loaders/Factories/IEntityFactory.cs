using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace VODB.Core.Loaders.Factories
{
    internal class EntityProxyFactory : IEntityFactory
    {

        static ProxyGenerator proxyGenerator = new ProxyGenerator();

        public Object Make(Type type, IInternalSession session)
        {
            if (type.Namespace.Equals("Castle.Proxies"))
            {
                return Make(type.BaseType, session);
            }

            return proxyGenerator.CreateClassProxy(type, new Interceptor(session));
        }
    }

    interface IEntityFactory
    {
        Object Make(Type type, IInternalSession session);
    }
}
