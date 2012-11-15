using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VODB.Tests.Models.Northwind;
using VODB.EntityValidators;
using VODB.Exceptions;

namespace VODB.Tests
{
    [TestClass]
    public class EntityValidators_Tests
    {
        [TestMethod, ExpectedException(typeof(ValidationException))]
        public void RequiredFields_Test()
        {
            try
            {
                new RequiredFieldsValidator()
                    .Validate(new Employee());
            }
            catch (ValidationException ex)
            {

                Assert.AreEqual(
                    "Required fields not set: { [LastName], [FirstName] }",
                    ex.Message);

                throw;
            }

        }

        [TestMethod, ExpectedException(typeof(ValidationException))]
        public void KeysFilled_Test()
        {
            try
            {
                new KeyFilledValidator()
                    .Validate(new Employee());
            }
            catch (ValidationException ex)
            {

                Assert.AreEqual(
                    "Key fields not set: { [EmployeeId] }",
                    ex.Message);

                throw;
            }

        }

        [TestMethod]
        public void KeysFilled_Ok_Test()
        {
            new KeyFilledValidator()
                .Validate(new Employee() { EmployeeId = 1 });
        }

        [TestMethod]
        public void RequiredFields_Ok_Test()
        {
            new RequiredFieldsValidator()
                .Validate(new Employee
                {
                    EmployeeId = 1,
                    LastName = "1",
                    FirstName = "1"
                });
        }
    }
}
