using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace VODB.Expressions
{
    /// <summary>
    /// Decodes Lambda expressions into expression parts.
    /// </summary>
    internal interface IExpressionDecoder
    {
        /// <summary>
        /// Gets the type of the node.
        /// </summary>
        /// <value>
        /// The type of the node.
        /// </value>
        ExpressionType NodeType { get; }
        /// <summary>
        /// Decodes the left part of the expression.
        /// </summary>
        /// <returns></returns>
        IEnumerable<ExpressionPiece> DecodeLeft();
        /// <summary>
        /// Decodes the right part of the expression.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Object> DecodeRight();
    }
}
