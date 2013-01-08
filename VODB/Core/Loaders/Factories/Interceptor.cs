using System.Diagnostics;
using Castle.DynamicProxy;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VODB.Core.Execution.Executers;
using VODB.Core.Execution.SqlPartialBuilders;
using VODB.Core.Infrastructure;

namespace VODB.Core.Loaders.Factories
{

    class ObjWrapper
    {
        public Object Value { get; set; }

        public Boolean IsLoaded { get; set; }
    }

    class Interceptor : IInterceptor
    {
        private readonly IInternalSession _Session;


        IDictionary<MethodInfo, ObjWrapper> lastResult = new Dictionary<MethodInfo, ObjWrapper>();

        public Interceptor(IInternalSession session)
        {
            _Session = session;
        }

        public void Intercept(IInvocation invocation)
        {
            invocation.Proceed();

            var method = invocation.Method;
            var fieldName = invocation.Method.Name.Remove(0, 4);

            if (method.Name.StartsWith("get_"))
            {
                GetValueHandler(invocation, method, fieldName);
            }
            else if (method.Name.StartsWith("set_"))
            {
                SetValueHandler(invocation, fieldName);
            }

        }

        private void SetValueHandler(IInvocation invocation, string fieldName)
        {
            var methodInfo = invocation.TargetType.GetMethod("get_" + fieldName);

            var value = invocation.GetArgumentValue(0);
            lastResult[methodInfo] = new ObjWrapper { Value = invocation.GetArgumentValue(0) };
        }

        private void GetValueHandler(IInvocation invocation, MethodInfo method, string fieldName)
        {
            ObjWrapper result;
            if (lastResult.TryGetValue(method, out result) && result.IsLoaded)
            {
                invocation.ReturnValue = result.Value;
                return;
            }

            result = lastResult[method] = ResolveResult(invocation, fieldName);
            invocation.ReturnValue = result.Value;
        }

        private static IEnumerable<T> ProxyGenericIterator<T>(
            object target, IEnumerable enumerable)
        {
            return ProxyNonGenericIterator(target, enumerable).Cast<T>();
        }

        private static IEnumerable ProxyNonGenericIterator(
            object target, IEnumerable enumerable)
        {
            return enumerable.Cast<object>();
        }

        private static readonly MethodInfo ProxyGenericIteratorMethod =
            typeof(Interceptor)
                .GetMethod(
                    "ProxyGenericIterator",
                    BindingFlags.NonPublic | BindingFlags.Static);

        private ObjWrapper ResolveResult(IInvocation invocation, string fieldName)
        {
            if (invocation.Method.ReturnType.IsGenericType)
            {
                // Todo: this code is kind of strange... Aply some refectoring strategy here.


                var callerTable = Engine.GetTable(invocation.Method.ReflectedType);
                var field = callerTable.FindCollectionField(fieldName);

                Debug.Assert(field != null, "field != null");

                var builder = new SqlQueryBuilder(field);

                var entityType = invocation.Method.ReturnType.GetGenericArguments()[0];

                var foreignTable = Engine.GetTable(entityType);

                foreach (var keyField in callerTable.KeyFields)
                {
                    builder.AddCondition(
                        foreignTable.FindField(keyField.FieldName),
                        GetValue(keyField, invocation.InvocationTarget, keyField.FieldName));
                }


                var method = ProxyGenericIteratorMethod.MakeGenericMethod(entityType);

                return new ObjWrapper
                {
                    Value = method.Invoke(null, new[] { invocation.InvocationTarget,
                        Engine.Get<IQueryExecuter>().RunQuery(
                            entityType,
                            _Session,
                            builder.Query,
                            builder.Parameters),
                    }), IsLoaded = true
                };
            }
            else
            {
                return new ObjWrapper { Value = _Session.GetById(invocation.ReturnValue), IsLoaded = true };
            }
        }

        private static Object GetValue(Field field, Object entity, String fieldName)
        {

            var value = field.GetValue(entity);

            Type valueType = value.GetType();

            if (Engine.IsMapped(valueType)) // Is a Mapped Entity
            {
                var table = Engine.GetTable(valueType);
                return GetValue(table.FindField(fieldName), value, fieldName);
            }

            return value;
        }

    }
}
