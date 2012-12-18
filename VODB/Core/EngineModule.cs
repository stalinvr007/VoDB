using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VODB.Core.Infrastructure;
using VODB.Infrastructure;

namespace VODB.Core
{

    internal class InfrastructureModule : NinjectModule
    {

        public override void Load()
        {
            Bind(typeof(IFieldMapping<>)).To(typeof(FieldMapping<>)).InSingletonScope();
        }
    }

    internal class EngineModule : NinjectModule
    {
        public override void Load()
        {

        }
    }
}
