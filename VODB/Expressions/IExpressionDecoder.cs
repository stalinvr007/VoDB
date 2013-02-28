using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace VODB.Expressions
{
    internal interface IExpressionDecoder
    {
        ExpressionType NodeType { get; }
        IEnumerable<ExpressionPart> DecodeLeft();
        IEnumerable<Object> DecodeRight();
    }
}
