using System;
using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace VODB.Core.Loaders.Factories
{
    internal class EntityProxyFactory : IEntityFactory
    {
        private static readonly ProxyGenerator proxyGenerator = new ProxyGenerator();

        #region IEntityFactory Members

        public Object Make(Type type, IInternalSession session)
        {
            return AsyncMake(type, session).Result;
        }

        public Task<object> AsyncMake(Type type, IInternalSession session)
        {
            return Task<Object>.Factory.StartNew(() =>
                                                     {
                                                         return type.Namespace.Equals("Castle.Proxies")
                                                                    ? Make(type.BaseType, session)
                                                                    : proxyGenerator.CreateClassProxy(type,
                                                                                                      new Interceptor(
                                                                                                          session));
                                                     });
        }

        #endregion
    }

    internal interface IEntityFactory
    {
        Object Make(Type type, IInternalSession session);

        Task<Object> AsyncMake(Type type, IInternalSession session);
    }
}