using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VODB.Core;
using VODB.Core.Infrastructure;

namespace VODB.Expressions
{
    internal interface IExpressionDecoder
    {
        IEnumerable<ExpressionPart> DecodeLeft();
        Object DecodeRight();
    }
}
