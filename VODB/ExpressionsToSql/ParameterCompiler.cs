using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VODB.Expressions;

namespace VODB.ExpressionsToSql
{
    class ParameterCompiler : ICompiler
    {
        private readonly int _Number;
        private const string PARAM_MASK = " = @p{0}";

        public ParameterCompiler(int number)
        {
            _Number = number;
        }

        public String Compile(int depth, IList<IExpressionPiece> pieces, ICompiler nextCompiler)
        {
            return String.Format(PARAM_MASK, _Number);
        }
    }
}
