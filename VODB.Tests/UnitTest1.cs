using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Fasterflect;

namespace VODB.Tests
{

    [TestFixture]
    public class UnitTest1
    {

        class TestClass
        {
            public int Id { get; set; }
        }

        [Test]
        public void DynamicFunction()
        {
            var property = typeof(TestClass).GetProperty("Id");

            var setter = property.DelegateForSetPropertyValue();

            var entity = new TestClass();
            for (int i = 0; i < 1000000; i++)
            {
                setter(entity, 10);                
                Assert.AreEqual(10, entity.Id);
            }
        }

        [Test]
        public void ReflectionFunction()
        {
            var property = typeof(TestClass).GetProperty("Id");

            var entity = new TestClass();
            for (int i = 0; i < 1000000; i++)
            {
                property.SetValue(entity, 10, null);
                Assert.AreEqual(10, entity.Id);
            }
        }
                
        static Delegate GetPropertySetter(Type entity, PropertyInfo pi)
        {
            MethodInfo mi = pi.GetSetMethod();

            ParameterExpression oParam = Expression.Parameter(entity, "obj");
            ParameterExpression vParam = Expression.Parameter(pi.PropertyType, "val");

            MethodCallExpression mce = Expression.Call(oParam, mi, Expression.Convert(vParam, pi.PropertyType));

            var assign = Expression.Assign(oParam, Expression.Convert(vParam, pi.PropertyType));

            return Expression.Lambda(assign, oParam, vParam).Compile();
        }

    }


}
