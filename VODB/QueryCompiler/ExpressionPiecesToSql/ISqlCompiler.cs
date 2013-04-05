using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VODB.Expressions;
using VODB.ExpressionsToSql;
using VODB.Infrastructure;

namespace VODB.QueryCompiler.ExpressionPiecesToSql
{
   
    public interface ISqlCompiler
    {
        /// <summary>
        /// Compiles the specified pieces.
        /// </summary>
        /// <returns></returns>
        String Compile();

        
    }
}
