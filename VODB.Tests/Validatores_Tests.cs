using System;
using NUnit.Framework;
using VODB.EntityValidators;
using VODB.Tests.Models.Northwind;
using VODB.Exceptions;

namespace VODB.Tests
{
    [TestFixture]
    public class Validatores_Tests
    {
        [Test, ExpectedException(typeof(ValidationException))]
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
    }
}
