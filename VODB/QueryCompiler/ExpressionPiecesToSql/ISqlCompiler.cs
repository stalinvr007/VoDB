using System;

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
