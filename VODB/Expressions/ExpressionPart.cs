using System;
using System.Linq.Expressions;
using VODB.Core.Infrastructure;

namespace VODB.Expressions
{
    class ExpressionPart
    {
        public String PropertyName { get; set; }
        public Field Field { get; set; }
        public Type EntityType { get; set; }
        public Table EntityTable { get; set; }
        public ExpressionType NodeType { get; set; }
    }
}
