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
            Config.RegisterExceptionHandler(new OnPrimaryKeyExceptionHandler())
                .RegisterExceptionHandler(new OnTruncatedExceptionHandler())
                .RegisterExceptionHandler(new OnUniqueKeyExceptionHandler());
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

            yield return new TestCaseData((Action<ISession>)(s =>
            {

                s.GetAll<Employee>().Where("aspodkaspdok").Count();

            })).Throws(typeof(VodbException))
            .SetName("Wrong query must throw VodbException.");
        }


        [TestCaseSource("GetExecutions")]
        public void Exception_System_Assert(Action<ISession> execution)
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
