﻿using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VODB.Core.Loaders.Factories;

namespace VODB.Sessions.EntityFactories
{
    class ProxyCreator : IEntityFactory
    {
        private static readonly ProxyGenerator proxyGenerator = new ProxyGenerator();

        public object Make(Type type, IInternalSession session)
        {
            return proxyGenerator.CreateClassProxy(type, 
                new ProxyGenerationOptions {
                    Selector = new InterceptorSelector(new LazyForeignProperty(session)) 
                });
        }
    }
}