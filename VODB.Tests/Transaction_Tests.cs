using NUnit.Framework;
using VODB.Tests.Models.Northwind;

namespace VODB.Tests
{
    [TestFixture]
    public class Transaction_Tests
    {
        [Test]
        public void InnerTransactions()
        {

            using (var session = new Session())
            {

                using (session.BeginTransaction())
                {
                    Employee sergio;
                    Employee sergio1;

                    var trans = session.BeginTransaction();
                    {
                        sergio = session.Insert(new Employee
                        {
                            LastName = "Ferreira",
                            FirstName = "Sérgio"
                        });

                        var trans1 = session.BeginTransaction();
                        {
                            sergio1 = session.Insert(new Employee
                            {
                                LastName = "Ferreira",
                                FirstName = "Sérgio"
                            });

                            Assert.IsTrue(session.Exists(sergio1));
                        }
                        trans1.RollBack();

                        Assert.IsTrue(session.Exists(sergio));
                        Assert.IsFalse(session.Exists(sergio1));
                    }
                    trans.RollBack();

                    Assert.IsFalse(session.Exists(sergio));
                }

            }

        }
    }
}
