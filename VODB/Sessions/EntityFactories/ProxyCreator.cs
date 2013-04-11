using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VODB.Core.Loaders.Factories;
using VODB.EntityTranslation;

namespace VODB.Sessions.EntityFactories
{
    class ProxyCreator : IEntityFactory
    {
        private static readonly ProxyGenerator proxyGenerator = new ProxyGenerator();

        public object Make(Type type, IInternalSession session, IEntityTranslator translator)
        {
            if (type.GetInterfaces().Contains(typeof(IProxyTargetAccessor)))
            {
                return Make(type.BaseType, session, translator);
            }

            return proxyGenerator.CreateClassProxy(type, 
                new ProxyGenerationOptions {
                    Selector = new InterceptorSelector(
                        new NonCollectionInterceptor(session),
                        new LazyCollectionForeignProperty(session, translator)
                    ) 
                });
        }
    }
}
