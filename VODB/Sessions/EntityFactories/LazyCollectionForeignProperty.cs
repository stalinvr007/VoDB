using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Fasterflect;
using System.Collections;
using VODB.Core.Execution.Executers;
using VODB.EntityTranslation;
using VODB.ExpressionsToSql;

namespace VODB.Sessions.EntityFactories
{
    internal class LazyCollectionForeignProperty : IFieldInterceptor
    {
        private static readonly MethodInfo ProxyGenericIteratorMethod =
            typeof(LazyCollectionForeignProperty)
                .GetMethod(
                    "ProxyGenericIterator",
                    BindingFlags.NonPublic | BindingFlags.Static);

        private static readonly MethodInfo SessionGenericGetAllMethod =
            typeof(ISession)
                .GetMethod(
                    "GetAll",
                    BindingFlags.Public | BindingFlags.Instance);

        private readonly IInternalSession _Session;
        private readonly IDictionary<MethodInfo, Object> lastResult = new Dictionary<MethodInfo, Object>();
        private readonly IEntityTranslator _Translator;


        public LazyCollectionForeignProperty(IInternalSession session, IEntityTranslator translator)
        {
            _Translator = translator;
            _Session = session;

        }

        public Boolean InterceptCollections { get { return true; } }

        private void SetResult(IInvocation invocation, MethodInfo method)
        {
            // Finds the entity type of the property return generic IEnumerable type.
            var entityType = method.ReturnParameter.ParameterType.GetGenericArguments().First();


            MethodInfo methodIterator = ProxyGenericIteratorMethod.MakeGenericMethod(entityType);

            // Gets the session GetAll method
            MethodInfo me = SessionGenericGetAllMethod.MakeGenericMethod(entityType);

            // Gets a new IQuery created using the session GetAll method.
            IQuery result = (IQuery)methodIterator.Invoke(null, new Object[] 
            {
                invocation.InvocationTarget,
                me.Invoke(_Session, new Object[]{ })
            });

            var foreignTable = _Translator.Translate(entityType);
            var callerTable = _Translator.Translate(invocation.Method.ReflectedType);

            foreach (var key in callerTable.Keys)
            {
                var foreignField = foreignTable.Fields
                    .Where(f => f.BindToField != null)
                    .FirstOrDefault(f => f.BindOrName.Equals(key.Name, StringComparison.InvariantCultureIgnoreCase));

                if (foreignField != null)
                {
                    result.InternalWhere(
                        foreignField,
                        new QueryParameter {
                            Field = key,
                            Name = foreignField.BindOrName,
                            Value = key.GetFieldFinalValue(invocation.InvocationTarget),
                            type = foreignField.Info.PropertyType
                        });
                }

            }
            
            lastResult[method] = invocation.ReturnValue = result;
        }

        public void Intercept(IInvocation invocation)
        {
            invocation.Proceed();

            MethodInfo method = invocation.Method;

            if (method.Name.StartsWith("set_"))
            {
                // Saves the setted value.
                lastResult[method] = invocation.ReturnValue;
                return;
            }

            if (method.Name.StartsWith("get_"))
            {
                // trys to get a cached value.
                object result = null;
                if (lastResult.TryGetValue(method, out result))
                {
                    invocation.ReturnValue = result;
                    return;
                }

                SetResult(invocation, method);
            }
        }

        private static IEnumerable<T> ProxyGenericIterator<T>(object target, IEnumerable enumerable)
        {
            return ProxyNonGenericIterator(target, enumerable).Cast<T>();
        }

        private static IEnumerable ProxyNonGenericIterator(object target, IEnumerable enumerable)
        {
            return enumerable.Cast<object>();
        }
    }
}
