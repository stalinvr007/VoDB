using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VODB.Exceptions;
using VODB.Expressions;

namespace VODB.QueryCompiler.ExpressionPiecesToSql
{
    class PiecesCompiler : ISqlCompiler
    {
        private const string RIGHT_BRACKET = "]";
        private const string LEFT_BRACKET = "[";

        private ISqlCompiler _Next;
        private IEnumerable<IExpressionPiece> _Pieces;

        public PiecesCompiler(IEnumerable<IExpressionPiece> pieces, ISqlCompiler next)
        {
            _Next = next;
            _Pieces = pieces;
        }

        public virtual String Compile()
        {
            var previous = _Pieces.First();

            var sb = new StringBuilder()
                .Append(LEFT_BRACKET).Append(previous.Field.Name).Append(RIGHT_BRACKET);

            int count = 0;
            foreach (var piece in _Pieces.Skip(1))
            {

                sb.Append(" in (Select [")
                    .Append(previous.Field.BindOrName)
                    .Append("] From [")
                    .Append(piece.EntityTable.Name)
                    .Append("] Where ")
                    .Append(LEFT_BRACKET).Append(piece.Field.Name).Append(RIGHT_BRACKET);

                previous = piece;
                ++count;
            }

            sb.Append(_Next.Compile());

            for (int i = 0; i < count; i++)
            {
                sb.Append(")");
            }

            return sb.ToString();
        }
    }
}
