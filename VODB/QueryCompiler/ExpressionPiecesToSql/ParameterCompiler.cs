using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using VODB.Expressions;
using VODB.ExpressionsToSql;

namespace VODB.QueryCompiler.ExpressionPiecesToSql
{
    class ParameterCompiler : ISqlCompiler
    {
        private readonly ExpressionType _NodeType;
        private readonly Func<Object, String> _ParameterAdder;
        private readonly Object _Value;
        
        public ParameterCompiler(Func<Object, String> parameterAdder, ExpressionType nodeType, Object value)
        {
            _Value = value;
            _ParameterAdder = parameterAdder;
            _NodeType = nodeType;
        }

        public ParameterCompiler(Func<Object, String> parameterAdder, Object value)
            : this(parameterAdder, ExpressionType.Equal, value)
        {

        }

        public ParameterCompiler(Func<Object, String> parameterAdder, Object value, ExpressionType type)
            : this(parameterAdder, type, value)
        {

        }

        public String Compile()
        {
            string parameter = _ParameterAdder(_Value);

            switch (_NodeType)
            {
                case ExpressionType.Equal: return " = " + parameter;
                case ExpressionType.GreaterThan: return " > " + parameter;
                case ExpressionType.GreaterThanOrEqual: return " >= " + parameter;
                case ExpressionType.LessThan: return " < " + parameter;
                case ExpressionType.LessThanOrEqual: return " <= " + parameter;
                case ExpressionType.NotEqual: return " != " + parameter;
                case ExpressionType.Parameter: return parameter;
                default: return " = " + parameter;
            }

        }
    }
}
