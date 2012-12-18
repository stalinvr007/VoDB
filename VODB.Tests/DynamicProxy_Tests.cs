using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VODB.Tests.Models.Northwind;
using System.Diagnostics;
using Castle.Components.DictionaryAdapter;
using System.Collections;
using Castle.DynamicProxy;

namespace VODB.Tests
{
    [Serializable]
    public class Interceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        { 

            invocation.Proceed();
            invocation.ReturnValue = "This is my result.";

        }
    }

    public class HelloWorld
    {
        public virtual string Message { get; set; }
    }

    [TestClass]
    public class DynamicProxy_Tests
    {
        [TestMethod]
        public void SimpleInterceptor_ResultChanger()
        {
            var proxyGenerator = new ProxyGenerator();
            var hw = proxyGenerator.CreateClassProxy<HelloWorld>(new Interceptor());
            Assert.AreEqual("This is my result.", hw.Message);
        }
    }


}
