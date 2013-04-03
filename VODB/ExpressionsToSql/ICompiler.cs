using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VODB.Expressions;

namespace VODB.ExpressionsToSql
{

    public interface ICompiler
    {
        String Compile(int depth, IList<IExpressionPiece> pieces, ICompiler nextCompiler);
    }
}