using Castle.DynamicProxy;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        
        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            var isCollection = 
                IsCollectionReturnType(method) ||
                IsCollectionParameterType(method);

            return _Interceptors.Where(e => e.InterceptCollections == isCollection).ToArray();
        }

        private static bool IsCollectionParameterType(MethodInfo method)
        {
            return method.Name.StartsWith("set_") && method.GetParameters().FirstOrDefault().ParameterType.GetGenericArguments().Count() > 0;
        }

        private static bool IsCollectionReturnType(MethodInfo method)
        {
            return method.Name.StartsWith("get_") && method.ReturnType.IsGenericType;
        }

    }
}
