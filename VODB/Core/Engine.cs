using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VODB.Infrastructure;

namespace VODB.Core
{
    internal static class Engine
    {

        static IKernel kernel = new StandardKernel(new InfrastructureModule());

        public static TClass Get<TClass>()
        {
            return kernel.Get<TClass>();
        }

        
    }
}
