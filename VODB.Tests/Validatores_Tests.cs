using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VODB.EntityValidators;
using VODB.Tests.Models.Northwind;
using VODB.Exceptions;

namespace VODB.Tests
{
    [TestClass]
    public class Validatores_Tests
    {
        [TestMethod, ExpectedException(typeof(ValidationException))]
        public void RequiredFields_Employee()
        {
            try
            {
                new RequiredFieldsValidator()
                    .Validate(new Employee());
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Required fields not set: { [LastName], [FirstName] }", ex.Message);
                throw;
            }
            

        }

        [TestMethod, ExpectedException(typeof(TruncatedException))]
        public void BigNamesThrowException()
        {
            Utils.EagerExecute(session =>
            {
                session.Insert(new Employee
                {
                    FirstName = "adoaskdpoaskdapsdkapsdokapsdokaspdTruncatedExceptionHandleradoaskdpoaskdapsdkapsdokapsdokaspdTruncatedExceptionHandleradoaskdpoaskdapsdkapsdokapsdokaspdTruncatedExceptionHandleradoaskdpoaskdapsdkapsdokapsdokaspdTruncatedExceptionHandleradoaskdpoaskdapsdkapsdokapsdokaspdTruncatedExceptionHandleradoaskdpoaskdapsdkapsdokapsdokaspdTruncatedExceptionHandlerok",
                    LastName = "adsads"
                });

            });
        }
    }
}
