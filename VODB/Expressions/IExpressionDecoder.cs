using System;
using System.Collections.Generic;

namespace VODB.Expressions
{
    internal interface IExpressionDecoder
    {
        IEnumerable<ExpressionPart> DecodeLeft();
        IEnumerable<Object> DecodeRight();
    }
}
