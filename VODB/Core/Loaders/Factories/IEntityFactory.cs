using System.Diagnostics;
using Castle.DynamicProxy;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace VODB.Core.Loaders.Factories
{
    internal class EntityProxyFactory : IEntityFactory
    {
        static readonly ProxyGenerator proxyGenerator = new ProxyGenerator();
        
        public Object Make(Type type, IInternalSession session)
        {
            return AsyncMake(type, session).Result;
        }

        public Task<object> AsyncMake(Type type, IInternalSession session)
        {
            return Task<Object>.Factory.StartNew(() =>
            {
                return type.Namespace.Equals("Castle.Proxies") ?
                    Make(type.BaseType, session) :
                    proxyGenerator.CreateClassProxy(type, new Interceptor(session));    
            });
        }
    }

    interface IEntityFactory
    {
        Object Make(Type type, IInternalSession session);

        Task<Object> AsyncMake(Type type, IInternalSession session);
    }
}
