using Castle.DynamicProxy;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VODB.Sessions.EntityFactories
{
    class InterceptorSelector : IInterceptorSelector
    {

        private readonly IFieldInterceptor[] _CollectionInterceptors;
        private readonly IFieldInterceptor[] _ForeignFieldInterceptors;

        public InterceptorSelector(params IFieldInterceptor[] interceptors)
        {
            _CollectionInterceptors = interceptors.Where(i => i.InterceptCollections).ToArray();
            _ForeignFieldInterceptors = interceptors.Where(i => !i.InterceptCollections).ToArray();
        }
        
        public IInterceptor[] SelectInterceptors(Type type, System.Reflection.MethodInfo method, IInterceptor[] interceptors)
        {
            var isCollection = method.ReturnType.GetInterfaces().Contains(typeof(IEnumerable)) ||
                method.GetParameters().FirstOrDefault().ParameterType.GetGenericArguments().Count() > 0;
            
            return isCollection ?
                _CollectionInterceptors :
                _ForeignFieldInterceptors;
        }
    }
}
