﻿using NUnit.Framework;
using VODB.DbLayer;

namespace VODB.Tests.ConnectionLayer
{
    [TestFixture]
    public class VodbConnection_Tests
    {

        [Test]
        public void Connection_BeginTransaction()
        {
            using (var connection = new VodbConnection(Utils.ConnectionCreator))
            {
                Assert.That(connection.BeginTransaction(), Is.Not.Null);
            }
        }

        [Test]
        public void Connection_CantClose_From_Inner_Transaction()
        {
            using (var connection = new VodbConnection(Utils.ConnectionCreator))
            {
                connection.BeginTransaction();
                connection.BeginTransaction();
                connection.Close();
                Assert.That(connection.IsOpened, Is.True);
            }
        }

        [Test]
        public void Connection_CantClose_From_Inner_Transactions()
        {
            using (var connection = new VodbConnection(Utils.ConnectionCreator))
            {
                var transaction = connection.BeginTransaction();
                for (int i = 0; i < 10; i++)
                {
                    var inner = connection.BeginTransaction();
                    
                    // Should not close!
                    connection.Close();
                    Assert.That(connection.IsOpened, Is.True);

                    // Should not influence the connection or Transaction.
                    inner.Commit();
                }

                // Ends the transaction by Rollback.
                transaction.Rollback();
                // Then the close statement has efect.
                connection.Close();
                Assert.That(connection.IsOpened, Is.False);
            }
        }

        [Test]
        public void Connection_BeginTransaction_And_Rollback()
        {
            using (var connection = new VodbConnection(Utils.ConnectionCreator))
            {
                var transaction = connection.BeginTransaction();
                try
                {
                    Assert.That(GetEmployeesCount(connection), Is.EqualTo(9));

                    connection.ExecuteNonQuery(
                        connection.MakeCommand("Insert Into Employees (FirstName, LastName) values ('testing', 'testing')")
                    );

                    Assert.That(GetEmployeesCount(connection), Is.EqualTo(10));
                }
                finally
                {
                    transaction.Rollback();
                }
            }
        }

        private static int GetEmployeesCount(VodbConnection connection)
        {
            return (int)connection.ExecuteScalar(connection.MakeCommand("Select count(*) From Employees"));
        }

        [Test]
        public void Connection_Close_NoOpen()
        {
            var connection = new VodbConnection(Utils.ConnectionCreator);
            connection.Close();
            Assert.That(connection.IsOpened, Is.False);
        }

        [Test]
        public void Connection_IsOpened()
        {
            var connection = new VodbConnection(Utils.ConnectionCreator);
            connection.Open();
            Assert.That(connection.IsOpened, Is.True);
            connection.Close();
            Assert.That(connection.IsOpened, Is.False);
        }

        [Test]
        public void Connection_IsNotOpen_After_Dispose()
        {
            VodbConnection connection;
            using (connection = new VodbConnection(Utils.ConnectionCreator))
            {
                connection.Open();
                Assert.That(connection.IsOpened, Is.True);
            }
            Assert.That(connection.IsOpened, Is.False);
        }

        [Test]
        public void Connection_Reopen()
        {
            var connection = new VodbConnection(Utils.ConnectionCreator);
            for (int i = 0; i < 10; i++)
            {
                connection.Open();
                Assert.That(connection.IsOpened, Is.True);
                connection.Close();
                Assert.That(connection.IsOpened, Is.False);
            }
        }

        [Test]
        public void Connection_MakeCommand()
        {
            VodbConnection connection;
            using (connection = new VodbConnection(Utils.ConnectionCreator))
            {
                var command = connection.MakeCommand();
                Assert.That(command, Is.Not.Null);
                Assert.That(connection.IsOpened, Is.False);
            }
        }

    }
}
