using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VODB.Exceptions;
using VODB.Exceptions.Handling;
using VODB.Tests.Models.Northwind;

namespace VODB.Tests.ExceptionHandlers
{
    [TestFixture]
    public class ExceptionHandling_Tests
    {
        
        [TestFixtureSetUp]
        public void Init_Handlers() 
        {
            Config.RegisterExceptionHandler(new PrimaryKeyExceptionHandler())
                .RegisterExceptionHandler(new TruncatedExceptionHandler())
                .RegisterExceptionHandler(new UniqueKeyExceptionHandler());
        }

        private static IEnumerable GetExecutions()
        {
            yield return new TestCaseData((Action<ISession>)(s => {

                s.Insert(new Customers
                {
                    CustomerId = "ALFKI",
                    CompanyName = ""
                });

            })).Throws(typeof(PrimaryKeyViolationException))
            .SetName("Inserting a duplicated record, throws exception");

            yield return new TestCaseData((Action<ISession>)(s =>
            {

                s.Insert(new Customers
                {
                    CustomerId = "ALFKIALFKIALFKIALFKIALFKIALFKIALFKIALFKIALFKIALFKIALFKIALFKI",
                    CompanyName = ""
                });

            })).Throws(typeof(TruncatedException))
            .SetName("Inserting a value into customer bigger than field max length");
        }


        [TestCaseSource("GetExecutions")]
        public void Insert_Record_Throws_Exception(Action<ISession> execution)
        {
            using (var session = new Session())
            {
                session.WithRollback(s =>
                {
                    execution(s);
                });
            }
        }

    }
}
