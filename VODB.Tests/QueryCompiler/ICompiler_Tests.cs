using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VODB.Expressions;
using VODB.ExpressionsToSql;
using VODB.Tests.Models.Northwind;

namespace VODB.Tests.QueryCompiler
{
    [TestFixture]
    public class ICompiler_Tests
    {

        private IExpressionPiece MakePiece<TEntity>(String propertyName)
        {
            return Utils.MakePiece<TEntity>(propertyName);
        }

        private IEnumerable GetCompilers()
        {
            yield return new TestCaseData(new ForeignKeyInCompiler(), 1, new[] { MakePiece<Orders>("Employee"), MakePiece<Employee>("ReportsTo") }, new ParameterCompiler(1))
                .Returns(" In (Select [EmployeeId] From [Employees] Where [ReportsTo] = @p1)");
        }


        [TestCaseSource("GetCompilers")]
        public String ICompiler_Assert_Result(ICompiler compiler, int depth, IList<IExpressionPiece> pieces, ICompiler nextCompiler)
        {
            return compiler.Compile(depth, pieces, nextCompiler);
        }

    }
}
