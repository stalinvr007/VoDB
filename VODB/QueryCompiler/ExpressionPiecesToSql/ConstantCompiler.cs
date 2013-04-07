using System;

namespace VODB.QueryCompiler.ExpressionPiecesToSql
{
    class ConstantCompiler : ISqlCompiler
    {

        private readonly String _Val;

        public ConstantCompiler(String val, params object[] args)
        {
            _Val = String.Format(val, args);
        }

        public String Compile()
        {
            return _Val;
        }
    }

}
