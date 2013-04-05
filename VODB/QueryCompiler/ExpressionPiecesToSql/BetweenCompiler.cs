using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VODB.Expressions;
using VODB.ExpressionsToSql;
using VODB.Infrastructure;

namespace VODB.QueryCompiler.ExpressionPiecesToSql
{
    class BetweenCompiler : ISqlCompiler
    {
        private readonly ISqlCompiler _FirstParameter;
        private readonly ISqlCompiler _SecondParameter;

        public BetweenCompiler(ISqlCompiler firstParameter, ISqlCompiler secondParameter)
        {
            _SecondParameter = secondParameter;
            _FirstParameter = firstParameter;
        }

        public String Compile()
        {
            return String.Format(" Between {0} And {1}",
                _FirstParameter.Compile(),
                _SecondParameter.Compile());
        }
    }
}