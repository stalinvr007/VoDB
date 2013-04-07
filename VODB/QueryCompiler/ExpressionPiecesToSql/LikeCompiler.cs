using System;
using VODB.Core.Execution.Executers.DbResults;

namespace VODB.QueryCompiler.ExpressionPiecesToSql
{
    class LikeCompiler : ISqlCompiler
    {
        private readonly ISqlCompiler _FirstParameter;
        private readonly WildCard _Token;
        
        public LikeCompiler(ISqlCompiler firstParameter, WildCard token = WildCard.Both)
        {
            _Token = token;
            _FirstParameter = firstParameter;
        }

        public String Compile()
        {
            string mask;

            switch (_Token)
            {
                case WildCard.Left: mask = " Like '%' + {0}"; break;
                case WildCard.Right: mask = " Like {0} + '%'"; break;
                case WildCard.Both: mask = " Like '%' + {0} + '%'"; break;
                case WildCard.None: mask = " Like {0}"; break;
                default: mask = " Like '%' + {0} + '%'"; break;
            }
            return String.Format(mask, _FirstParameter.Compile());
        }
    }
}
