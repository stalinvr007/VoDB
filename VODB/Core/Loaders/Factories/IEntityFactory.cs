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
            return type.Namespace.Equals("Castle.Proxies")
                           ? Make(type.BaseType, session)
                           : proxyGenerator.CreateClassProxy(type,
                                                             new Interceptor(
                                                                 session));
        }

        #endregion
    }

    internal interface IEntityFactory
    {
        Object Make(Type type, IInternalSession session);
    }
}