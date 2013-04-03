using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VODB.Expressions;

namespace VODB.ExpressionsToSql
{
    class ForeignKeyInCompiler : ICompiler
    {
        private const string IN_SELECT = " In (Select [{0}] From [{1}] Where [{2}]{3})";

        public String Compile(int depth, IList<IExpressionPiece> pieces, ICompiler nextCompiler)
        {
            var previous = pieces[depth - 1];
            var current = pieces[depth];

            return String.Format(IN_SELECT,
                previous.Field.BindOrName,
                current.EntityTable.Name,
                current.Field.Name,
                nextCompiler.Compile(depth + 1, pieces, nextCompiler));
        }
    }
}
