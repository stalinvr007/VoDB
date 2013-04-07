using System;
using System.Collections.Generic;
using System.Linq;
using VODB.Expressions;
using VODB.Infrastructure;

namespace VODB.QueryCompiler.ExpressionPiecesToSql
{
    class SubQueryCompiler : ISqlCompiler
    {
        private readonly ISqlCompiler _Compiler;

        public SubQueryCompiler(ITable table, IEnumerable<IExpressionPiece> pieces, ISqlCompiler next)
        {
            var expressionPieces = pieces as List<IExpressionPiece> ?? pieces.ToList();
            _Compiler = new PiecesCompiler(expressionPieces,
                new InTableCompiler(table, expressionPieces.Last(), next));
        }

        public String Compile()
        {
            return _Compiler.Compile();
        }
    }
}
