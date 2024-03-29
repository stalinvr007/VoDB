using System;
using VODB.Core.Infrastructure;
using VODB.ExpressionParser;

namespace VODB.Core.Execution.SqlPartialBuilders
{
    internal class Parameter : Key
    {
        public Parameter(Field field, string paramName, Object value)
            : base(field, paramName)
        {
            Value = value;
        }

        public Object Value { get; set; }
    }
}