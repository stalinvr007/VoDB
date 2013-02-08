using NUnit.Framework;
using VODB.Tests.Models.Northwind;
using VODB.EntityValidators;
using VODB.Exceptions;

namespace VODB.Tests
{
    [TestFixture]
    public class EntityValidators_Tests
    {
        [Test, ExpectedException(typeof(ValidationException))]
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

        [Test, ExpectedException(typeof(ValidationException))]
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

        [Test]
        public void KeysFilled_Ok_Test()
        {
            new KeyFilledValidator()
                .Validate(new Employee { EmployeeId = 1 });
        }

        [Test]
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
