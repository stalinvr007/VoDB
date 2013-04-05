using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using VODB.Expressions;

namespace VODB.QueryCompiler.ExpressionPiecesToSql
{
    class ParameterCompiler : ISqlCompiler
    {
        private readonly Func<int> _GetNumber;
        private readonly ExpressionType _NodeType;
        
        public ParameterCompiler(Func<int> getNumber, ExpressionType nodeType)
        {
            _NodeType = nodeType;
            _GetNumber = getNumber;
        }

        public ParameterCompiler(Func<int> getNumber)
        {
            _GetNumber = getNumber;
            _NodeType = ExpressionType.Parameter;
        }

        public String Compile()
        {
            switch (_NodeType)
            {
                case ExpressionType.Equal: return " = @p" + _GetNumber();
                case ExpressionType.GreaterThan: return " > @p" + _GetNumber();
                case ExpressionType.GreaterThanOrEqual: return " >= @p" + _GetNumber();
                case ExpressionType.LessThan: return " < @p" + _GetNumber();
                case ExpressionType.LessThanOrEqual: return " <= @p" + _GetNumber();
                case ExpressionType.NotEqual: return " != @p" + _GetNumber();
                case ExpressionType.Parameter: return "@p" + _GetNumber();
                default: return " = @p" + _GetNumber();
            }
            
        }
    }
}
