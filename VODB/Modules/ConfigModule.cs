using Ninject;
using Ninject.Modules;
using VODB.VirtualDataBase;

namespace VODB.Modules
{

    static class Configuration
    {

        static readonly IKernel kernel = new StandardKernel(new ConfigModule());

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
