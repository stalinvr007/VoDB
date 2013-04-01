using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VODB.Sessions.EntityFactories
{
    interface IFieldInterceptor : IInterceptor
    {

        Boolean InterceptCollections { get; }

    }
}
