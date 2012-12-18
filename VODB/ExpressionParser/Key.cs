using System;
using VODB.Infrastructure;

namespace VODB.ExpressionParser
{
    public class Key
    {
        public Key(Field field, string paramName)
        {
            ParamName = paramName;
            Field = field;
        }

        public Field Field { get; private set; }

        public String ParamName { get; private set; }

    }
}