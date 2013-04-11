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


        private readonly IFieldInterceptor[] _Interceptors;
        public InterceptorSelector(params IFieldInterceptor[] interceptors)
        {
            _Interceptors = interceptors;            
        }
        
        public IInterceptor[] SelectInterceptors(Type type, System.Reflection.MethodInfo method, IInterceptor[] interceptors)
        {
            var isCollection = method.ReturnType.GetInterfaces().Contains(typeof(IEnumerable)) ||
                method.GetParameters().FirstOrDefault().ParameterType.GetGenericArguments().Count() > 0;

            return _Interceptors.Where(e => e.InterceptCollections == isCollection).ToArray();            
        }
    }
}
