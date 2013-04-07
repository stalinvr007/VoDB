using System;
using System.Linq;
using VODB.Expressions;
using VODB.Infrastructure;

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
            var field = _Table.Fields.FirstOrDefault(f => f.BindOrName == _Piece.Field.BindOrName);
            var fieldName = field != null ? field.Name : _Piece.Field.BindOrName;

            return String.Format(" In (Select [{0}] From [{1}]{2})",
                fieldName,
                _Table.Name,
                _Compiler.Compile());
        }
    }
}
