using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VODB.Expressions;
using VODB.ExpressionsToSql;
using VODB.Infrastructure;

namespace VODB.QueryCompiler.ExpressionPiecesToSql
{
    class SubQueryCompiler : ISqlCompiler
    {
        private ISqlCompiler _Compiler;

        public SubQueryCompiler(ITable table, IEnumerable<IExpressionPiece> pieces, ISqlCompiler next)
        {
            _Compiler = new PiecesCompiler(pieces,
                new InTableCompiler(table, pieces.Last(), next));
        }

        public String Compile()
        {
            return _Compiler.Compile();
        }
    }
}
