using Microsoft.VisualStudio.TestTools.UnitTesting;
using VODB.Sessions;
using VODB.Tests.Models.Northwind;

namespace VODB.Tests
{
    [TestClass]
    public class Transaction_Tests
    {
        [TestMethod]
        public void InnerTransactions()
        {

            using (var session = new Session(null, null))
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
