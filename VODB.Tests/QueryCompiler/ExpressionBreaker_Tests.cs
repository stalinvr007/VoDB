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
        private static IEntityTranslator _Translator = new EntityTranslator();
        private static IExpressionBreaker _Breaker = new ExpressionBreaker(_Translator);

        private IEnumerable GetExpressions()
        {
            yield return MakeExpression<Employee, int>(e => e.EmployeeId, MakePiece<Employee>("EmployeeId"));

            yield return MakeExpression<Employee, Employee>(e => e.ReportsTo, MakePiece<Employee>("ReportsTo"));
            
            yield return MakeExpression<Employee, int>(e => e.ReportsTo.EmployeeId, MakePiece<Employee>("EmployeeId"), MakePiece<Employee>("ReportsTo"));
            
            yield return MakeExpression<Orders, int>(e => e.Employee.ReportsTo.EmployeeId, MakePiece<Employee>("EmployeeId"), MakePiece<Employee>("ReportsTo"), MakePiece<Orders>("Employee"));
            
            yield return MakeExpression<Orders, Boolean>(e => e.Employee.ReportsTo.EmployeeId == 0, MakePiece<Employee>("EmployeeId"), MakePiece<Employee>("ReportsTo"), MakePiece<Orders>("Employee"));
            
            yield return MakeExpression<Orders, String>(e => e.Employee.ReportsTo.EmployeeId.ToString(), MakePiece<Employee>("EmployeeId"), MakePiece<Employee>("ReportsTo"), MakePiece<Orders>("Employee"))
                .Throws(typeof(UnableToGetTheFirstMember));

            yield return MakeExpression<Orders, String>(e => e.Employee.ReportsTo.Address, MakePiece<Employee>("Address"), MakePiece<Employee>("ReportsTo"), MakePiece<Orders>("Employee"));

            yield return MakeExpression<OrderDetails, String>(e => e.Order.Employee.ReportsTo.FirstName, MakePiece<Employee>("FirstName"), MakePiece<Employee>("ReportsTo"), MakePiece<Orders>("Employee"), MakePiece<OrderDetails>("Order"));
        }

        private TestCaseData MakeExpression<TEntity, TReturnValue>(
            Expression<Func<TEntity, TReturnValue>> expression,
            params IExpressionPiece[] pieces)
        {
            return new TestCaseData(_Breaker, expression, pieces);
        }

        private IExpressionPiece MakePiece<TEntity>(String propertyName)
        {
            var type = typeof(TEntity);
            var table = _Translator.Translate(type);

            return new ExpressionPiece
            {
                EntityType = type,
                EntityTable = table,
                PropertyName = propertyName,
                Field = table.Fields.First(f => f.Info.Name == propertyName)
            };
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
