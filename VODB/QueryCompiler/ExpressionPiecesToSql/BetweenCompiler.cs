using System;

namespace VODB.QueryCompiler.ExpressionPiecesToSql
{
    class BetweenCompiler : ISqlCompiler
    {
        private readonly ISqlCompiler _FirstParameter;
        private readonly ISqlCompiler _SecondParameter;
        
        public BetweenCompiler(ISqlCompiler parameterCompiler, ISqlCompiler secondParameter)
        {
            _SecondParameter = secondParameter;
            _FirstParameter = parameterCompiler;
        }

        public String Compile()
        {
            return String.Format(" Between {0} And {1}",
                _FirstParameter.Compile(),
                _SecondParameter.Compile());
        }
    }
}