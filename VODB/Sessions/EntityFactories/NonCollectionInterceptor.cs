using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Fasterflect;

namespace VODB.Sessions.EntityFactories
{
    class NonCollectionInterceptor : FieldInterceptorBase
    {
        private readonly IInternalSession _Session;

        public NonCollectionInterceptor(IInternalSession session)
            : base(false)
        {
            _Session = session;
        }

        protected override object GetFieldValue(MethodInfo method, IInvocation invocation)
        {
            // Uses the value allready set to reload it with fresh data.
            return _Session.GetById(invocation.ReturnValue);
        }
    }
}
