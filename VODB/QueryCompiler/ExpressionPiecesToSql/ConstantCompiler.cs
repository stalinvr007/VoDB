using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VODB.Expressions;
using VODB.ExpressionsToSql;

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
