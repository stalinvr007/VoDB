using Ninject;
using Ninject.Modules;
using Ninject.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VODB.VirtualDataBase;

namespace VODB.Modules
{

    static class Configuration
    {

        static IKernel kernel = new StandardKernel(new ConfigModule());

        public static TType GetInstance<TType>()
        {
            return kernel.Get<TType>();
        }
        
    }

    sealed class ConfigModule : NinjectModule
    {
    
        public override void Load()
        {

            Kernel.Bind<ITableCreator>().To<TableCreator>();

        }

    }
}
