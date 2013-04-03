using NUnit.Framework;
using System;
using System.Collections;
using System.Linq.Expressions;
using System.Linq;
using VODB.Expressions;
using VODB.Tests.Models.Northwind;
using System.Collections.Generic;
using VODB.EntityTranslation;
using VODB.Exceptions;

namespace VODB.Tests.QueryCompiler
{

    [TestFixture]
    public class ExpressionBreaker_Tests
    {

        private static IExpressionBreaker _Breaker = new ExpressionBreaker(Utils.Translator);

        private IEnumerable GetExpressions()
        {
            yield return MakeExpression<Employee, int>(e => e.EmployeeId, MakePiece<Employee>("EmployeeId"));

            yield return MakeExpression<Employee, Employee>(e => e.ReportsTo, MakePiece<Employee>("ReportsTo"));

            yield return MakeExpression<Employee, int>(e => e.ReportsTo.EmployeeId, MakePiece<Employee>("ReportsTo"), MakePiece<Employee>("EmployeeId"));

            yield return MakeExpression<Orders, int>(e => e.Employee.ReportsTo.EmployeeId, MakePiece<Orders>("Employee"), MakePiece<Employee>("ReportsTo"), MakePiece<Employee>("EmployeeId"));

            yield return MakeExpression<Orders, Boolean>(e => e.Employee.ReportsTo.EmployeeId == 0, MakePiece<Orders>("Employee"), MakePiece<Employee>("ReportsTo"), MakePiece<Employee>("EmployeeId"));

            yield return MakeExpression<Orders, String>(e => e.Employee.ReportsTo.EmployeeId.ToString())
                .Throws(typeof(UnableToGetTheFirstMember));

            yield return MakeExpression<Orders, String>(e => e.Employee.ReportsTo.Address, MakePiece<Orders>("Employee"), MakePiece<Employee>("ReportsTo"), MakePiece<Employee>("Address"));

            yield return MakeExpression<OrderDetails, String>(e => e.Order.Employee.ReportsTo.FirstName, MakePiece<OrderDetails>("Order"), MakePiece<Orders>("Employee"), MakePiece<Employee>("ReportsTo"), MakePiece<Employee>("FirstName"));
        }

        private TestCaseData MakeExpression<TEntity, TReturnValue>(Expression<Func<TEntity, TReturnValue>> expression, params IExpressionPiece[] pieces)
        {
            return new TestCaseData(_Breaker, expression, pieces);
        }

        private IExpressionPiece MakePiece<TEntity>(String propertyName)
        {
            return Utils.MakePiece<TEntity>(propertyName);
        }

        [TestCaseSource("GetExpressions")]
        public void ExpressionBreaker_Assert_Pieces(IExpressionBreaker breaker, LambdaExpression expression, IExpressionPiece[] expected)
        {
            var pieces = breaker.BreakExpression(expression).ToList();

            CollectionAssert.IsNotEmpty(pieces);
            CollectionAssert.AllItemsAreNotNull(pieces);

            Assert.That(pieces.Count, Is.EqualTo(expected.Length));

            int i = 0;

            foreach (var piece in pieces)
            {
                var expectedPiece = expected[i++];

                Assert.AreSame(piece.EntityTable, expectedPiece.EntityTable);
                Assert.AreSame(piece.EntityType, expectedPiece.EntityType);
                Assert.That(piece.Field, Is.Not.Null);
                Assert.That(piece.PropertyName, Is.EqualTo(expectedPiece.PropertyName));
            }
        }

    }
}
