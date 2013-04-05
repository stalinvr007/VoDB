using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VODB.Expressions;
using VODB.ExpressionsToSql;
using VODB.Infrastructure;
using VODB.Exceptions;

namespace VODB.QueryCompiler.ExpressionPiecesToSql
{
    class InTableCompiler : ISqlCompiler
    {
        private readonly ISqlCompiler _Compiler;
        private readonly IExpressionPiece _Piece;
        private readonly ITable _Table;

        public InTableCompiler(ITable table, IExpressionPiece piece, ISqlCompiler compiler)
        {
            _Table = table;
            _Piece = piece;
            _Compiler = compiler;
        }

        public String Compile()
        {
            return String.Format(" In (Select [{0}] From [{1}]{2})",
                _Piece.Field.BindOrName,
                _Table.Name,
                _Compiler.Compile());
        }
    }
}
