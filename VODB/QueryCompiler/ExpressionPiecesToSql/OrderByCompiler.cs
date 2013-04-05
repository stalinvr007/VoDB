using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VODB.Exceptions;
using VODB.Expressions;

namespace VODB.QueryCompiler.ExpressionPiecesToSql
{
    class OrderByCompiler : PiecesCompiler
    {
        private readonly IEnumerable<IExpressionPiece> _Pieces;

        public OrderByCompiler(IEnumerable<IExpressionPiece> pieces)
            : base(pieces, new ConstantCompiler(""))
        {
            _Pieces = pieces;
        }

        public override string Compile()
        {
            if (_Pieces.Count() > 1)
            {
                throw new OrderByClauseException("Can't receive more than one level.");
            }
            return base.Compile();
        }
    }
}
